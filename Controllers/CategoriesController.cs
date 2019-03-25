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

    // GET: api/Categories/0
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories(bool categoryType)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      List<Category> categories = await _context.Category.Where(c =>
        c.CategoryType == categoryType &&
        c.UserId.Equals(userId)
      ).ToListAsync();

      List<CategoryDto> categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
      return categoryDtos;
    }

    // GET: api/Categories/00000000-0000-0000-0000-000000000000
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      Category category = await _context.Category.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      if (category.UserId != userId)
      {
        return Unauthorized();
      }

      CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);
      return categoryDto;
    }

    // POST: api/Categories/search/0
    [HttpPost("search")]
    public async Task<ActionResult<List<CategoryDto>>> GetByName(bool categoryType, [FromBody] string categoryName)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      List<Category> categories = await _context.Category.Where(
        c => c.CategoryName.ToLower().StartsWith(categoryName.ToLower()) &&
          c.CategoryType == categoryType &&
          c.UserId.Equals(userId.Value)).ToListAsync();

      List<CategoryDto> categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
      return categoryDtos;
    }

    // POST: api/Categories
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto categoryDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

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
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      if (id != categoryDto.CategoryId)
      {
        return BadRequest("Category ID does not match the Posted object.");
      }

      Category category = await _context.Category.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }
      else if (category.UserId != userId)
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
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

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

      CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);
      return categoryDto;
    }
  }
}