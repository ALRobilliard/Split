using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic; 
using System.Linq; 
using System;
using SplitApi.Models;  

namespace SplitApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
  	public class AccountsController : ControllerBase
  	{
		private readonly SplitContext _context;

		public AccountsController (SplitContext context)
		{
			_context = context;

			if (_context.Accounts.Count() == 0)
			{
				Account sampleAccount = new Account
				{
					Id = new Guid(),
					Name = "Sample Account"
				};

				_context.Accounts.Add(sampleAccount);
				_context.SaveChanges();
			}
		}

		public ActionResult<List<Account>> GetAll()
		{
			return _context.Accounts.ToList();
		}
  	}
}