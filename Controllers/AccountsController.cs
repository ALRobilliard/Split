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
					Id = Guid.NewGuid(),
					Name = "Sample Account"
				};

				_context.Accounts.Add(sampleAccount);
				_context.SaveChanges();
			}
		}

		// GET: api/Accounts
		[HttpGet]
		public async Task<ActionResult<List<Account>>> GetAccounts()
		{
			return await _context.Accounts.ToListAsync();
		}

		// GET: api/Accounts/92b5bf46-6697-44b9-b1a4-33e6d09e9c6f
		[HttpGet("{id}")]
		public async Task<ActionResult<Account>> GetAccount(Guid id)
		{
			Account account = await _context.Accounts.FindAsync(id);

			if (account == null)
			{
				return NotFound();
			}

			return account;
		}

		// POST: api/Accounts
		[HttpPost]
		public async Task<ActionResult<Account>> PostAccount(Account account)
		{
			// Overwrite the ID with a new GUID for the Account to be inserted.
			account.Id = Guid.NewGuid();

			_context.Accounts.Add(account);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetAccount", new { Id = account.Id }, account);
		}
  	}
}