using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using SplitApi.Dtos;
using SplitApi.Extensions;
using SplitApi.Helpers;
using SplitApi.Models;

namespace SplitApi.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {
    private readonly SplitContext _context;
    private IMapper _mapper;

    public CategoriesController(IMapper mapper, SplitContext context)
    {
      _mapper = mapper;
      _context = context;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
      List<Category> categories = null;
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      if (userId != null)
      {
        categories = await _context.Category.Where(c => c.UserId.Equals(userId)).ToListAsync();
      }

      return _mapper.Map<List<CategoryDto>>(categories);
    }

    // GET: api/Categories/00000000-0000-0000-0000-000000000000
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      Category category = await _context.Category.FindAsync(id);

      if (category == null)
      {
        return NotFound();
      }

      if (category.UserId != userId)
      {
        return Unauthorized();
      }

      return _mapper.Map<CategoryDto>(category);
    }

    // POST: api/Categories
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto categoryDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      categoryDto.UserId = userId.Value;

      Category category = _mapper.Map<Category>(categoryDto);

      _context.Category.Add(category);
      await _context.SaveChangesAsync();

      // Refresh DTO
      categoryDto = _mapper.Map<CategoryDto>(category);

      return CreatedAtAction("GetCategory", new { Id = category.CategoryId }, categoryDto);
    }

    // PUT: api/Categories/00000000-0000-0000-0000-000000000000
    [HttpPut("{id}")]
    public async Task<ActionResult> PutCategory(Guid id, CategoryDto categoryDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      if (id != categoryDto.CategoryId)
      {
        return BadRequest();
      }

      Category category = await _context.Category.FindAsync(id);

      if (category.UserId != userId)
      {
        return Unauthorized();
      }

      category.CategoryName = categoryDto.CategoryName;
      category.CategoryType = categoryDto.CategoryType;
      category.ModifiedOn = DateTime.Now;
      _context.Entry(category).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/Categories/00000000-0000-0000-0000-000000000000
    [HttpDelete("{id}")]
    public async Task<ActionResult<CategoryDto>> DeleteCategory(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      Category category = await _context.Category.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      if (category.UserId != userId)
      {
        return Unauthorized();
      }

      _context.Category.Remove(category);
      await _context.SaveChangesAsync();

      return _mapper.Map<CategoryDto>(category);
    }
  }
}