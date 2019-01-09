using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic; 
using System.Linq; 
using System;
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
            if (_context.Categories.Count() == 0)             
            {                 
                Category sampleCategory = new Category 
                {
                    Id = new Guid(),
                    Name = "Sample Category",
                    CategoryType = "Expense"
                };

                _context.Categories.Add(sampleCategory);                 
                _context.SaveChanges();             
            }         
        }

        [HttpGet]
        public ActionResult<List<Category>> GetAll() 
        {
            return _context.Categories.ToList();
        }    
    } 
}