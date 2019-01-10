using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic; 
using System.Linq; 
using System;
using SplitApi.Models;  

namespace SplitApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TransactionsController : ControllerBase
	{
		private readonly SplitContext _context;

		public TransactionsController(SplitContext context)
		{
			_context = context;
		}

		[HttpGet]
		public ActionResult<List<Transaction>> GetAll()
		{
			return _context.Transactions.ToList();
		}
	}
}