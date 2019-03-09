using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using SplitApi.Models;

namespace SplitApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {
    private readonly SplitContext _context;

    public CategoriesController(SplitContext context)
    {
      _context = context;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetCategories()
    {
      return await _context.Category.ToListAsync();
    }

    // GET: api/Categories/00000000-0000-0000-0000-000000000000
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(Guid id)
    {
      Category category = await _context.Category.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      return category;
    }

    // POST: api/Categories
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
      _context.Category.Add(category);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetCategory", new { Id = category.CategoryId }, category);
    }

    // PUT: api/Categories/00000000-0000-0000-0000-000000000000
    [HttpPut("{id}")]
    public async Task<ActionResult> PutCategory(Guid id, Category category)
    {
      if (id != category.CategoryId)
      {
        return BadRequest();
      }

      _context.Entry(category).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/Categories/00000000-0000-0000-0000-000000000000
    [HttpDelete("{id}")]
    public async Task<ActionResult<Category>> DeleteCategory(Guid id)
    {
      Category category = await _context.Category.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      _context.Category.Remove(category);
      await _context.SaveChangesAsync();

      return category;
    }
  }
}