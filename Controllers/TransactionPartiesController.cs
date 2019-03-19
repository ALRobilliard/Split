using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic; 
using System.Linq; 
using System;
using SplitApi.Models;  

namespace SplitApi.Controllers 
{
	[Route("api/[controller]")]
	[ApiController]
	public class TransactionPartiesController : ControllerBase
	{
		private readonly SplitContext _context;

		public TransactionPartiesController(SplitContext context)
		{
			_context = context;

			if (_context.TransactionParties.Count() == 0)
			{
				TransactionParty sampleTransactionParty = new TransactionParty
				{
					Id = new Guid(),
					Name = "Sample Transaction Party"
				};

				_context.TransactionParties.Add(sampleTransactionParty);
				_context.SaveChanges();
			}
		}

		[HttpGet]
		public ActionResult<List<TransactionParty>> GetAll()
		{
			return _context.TransactionParties.ToList();
		}
	}
}