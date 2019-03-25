using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SplitApi.Models;
using SplitApi.Dtos;
using SplitApi.Extensions;
using AutoMapper;

namespace SplitApi.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class AccountsController : ControllerBase
  {
    private readonly SplitContext _context;
    private IMapper _mapper;

    public AccountsController(IMapper mapper, SplitContext context)
    {
      _mapper = mapper;
      _context = context;
    }

    // GET: api/Accounts
    [HttpGet]
    public async Task<ActionResult<List<AccountDto>>> GetAccounts()
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      List<Account> accounts = await _context.Account.Where(a => a.UserId.Equals(userId)).ToListAsync();

      List<AccountDto> accountDtos = _mapper.Map<List<AccountDto>>(accounts);
      return accountDtos;
    }

    // GET: api/Accounts/00000000-0000-0000-0000-000000000000
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccount(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      Account account = await _context.Account.FindAsync(id);
      if (account == null)
      {
        return NotFound();
      }
      else if (account.UserId != userId)
      {
        return Unauthorized();
      }

      AccountDto accountDto = _mapper.Map<AccountDto>(account);
      return accountDto;
    }

    // POST: api/Accounts
    [HttpPost]
    public async Task<ActionResult<AccountDto>> PostAccount([FromBody] string accountName)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      Account account = new Account();
      account.AccountName = accountName;

      // Add owning user.
      account.UserId = userId.Value;

      _context.Account.Add(account);
      await _context.SaveChangesAsync();

      AccountDto accountDto = _mapper.Map<AccountDto>(account);
      return CreatedAtAction("GetAccount", new { Id = account.AccountId }, accountDto);
    }

    // PUT: api/Accounts/00000000-0000-0000-0000-000000000000
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAccount(Guid id, AccountDto accountDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      if (id != accountDto.AccountId)
      {
        return BadRequest("Account ID does not match the Posted object.");
      }

      Account account = await _context.Account.FindAsync(id);
      if (account == null)
      {
        return NotFound();
      }

      if (account.UserId != userId)
      {
        return Unauthorized();
      }

      account.AccountName = accountDto.AccountName;
      account.ModifiedOn = DateTime.Now;
      _context.Entry(account).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/Accounts/00000000-0000-0000-0000-000000000000
    [HttpDelete("{id}")]
    public async Task<ActionResult<AccountDto>> DeleteAccount(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      Account account = await _context.Account.FindAsync(id);
      if (account == null)
      {
        return NotFound();
      }

      if (account.UserId != userId)
      {
        return Unauthorized();
      }

      _context.Account.Remove(account);
      await _context.SaveChangesAsync();

      AccountDto accountDto = _mapper.Map<AccountDto>(account);
      return accountDto;
    }
  }
}