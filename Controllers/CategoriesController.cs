using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic; 
using System.Linq; 
using System;
using split_api.Models;  

namespace split_api.Controllers 
{     
    [Route("api/[controller]")]     
    [ApiController]     
    public class CategoriesController : ControllerBase     
    {        
        private readonly transactionsContext _context;          

        public CategoriesController(transactionsContext context)         
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