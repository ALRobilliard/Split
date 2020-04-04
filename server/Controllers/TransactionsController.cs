using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Split.Dtos;
using Split.Extensions;
using Split.Models;
using Split.Services;

namespace Split.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class TransactionsController : ControllerBase
  {
    private IMapper _mapper;
    private readonly SplitContext _context;
    private readonly AccountService _accountService;

    public TransactionsController(IMapper mapper, SplitContext context)
    {
      _mapper = mapper;
      _context = context;
      _accountService = new AccountService(context);
    }

    // GET: api/Transactions
    [HttpGet]
    public async Task<ActionResult<List<TransactionDto>>> GetTransactions(DateTime startDate, DateTime endDate)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be read from token.");
      }

      if (startDate.CompareTo(endDate) > 0)
      {
        return BadRequest("Specified StartDate is later than Specified EndDate.");
      }

      List<Transaction> transactions = await _context.Transaction.Where(
        t => t.UserId.Equals(userId) &&
        t.TransactionDate.CompareTo(startDate) >= 0 &&
        t.TransactionDate.CompareTo(endDate) <= 0
        ).ToListAsync();

      List<TransactionDto> transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);

      // Set Linked Entity names for client access.
      foreach(var transaction in transactionDtos) {
        if (transaction.CategoryId != null) {
          Category category = await _context.Category.FindAsync(transaction.CategoryId);
          transaction.CategoryName = category.CategoryName;
        }

        if (transaction.TransactionPartyId != null) {
          TransactionParty transactionParty = await _context.TransactionParty.FindAsync(transaction.TransactionPartyId);
          transaction.TransactionPartyName = transactionParty.TransactionPartyName;
        }

        if (transaction.AccountInId != null) {
          Account account = await _context.Account.FindAsync(transaction.AccountInId);
          transaction.AccountInName = account.AccountName;
        }

        if (transaction.AccountOutId != null) {
          Account account = await _context.Account.FindAsync(transaction.AccountOutId);
          transaction.AccountOutName = account.AccountName;
        }
      };

      return transactionDtos;
    }

    // GET: api/Transactions/00000000-0000-0000-0000-000000000000
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetTransaction(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be read from token.");
      }

      Transaction transaction = await _context.Transaction.FindAsync(id);
      if (transaction == null)
      {
        return NotFound();
      }
      else if (transaction.UserId != userId)
      {
        return Unauthorized();
      }

      TransactionDto transactionDto = _mapper.Map<TransactionDto>(transaction);
      return transactionDto;
    }

    // POST: api/Transactions
    [HttpPost]
    public async Task<ActionResult<TransactionDto>> PostTransaction(TransactionDto transactionDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be read from token.");
      }

      transactionDto.UserId = userId;

      Transaction transaction = _mapper.Map<Transaction>(transactionDto);

      _context.Transaction.Add(transaction);
      
      // Process balance change.
      if (transactionDto.AccountInId != null)
      {
        Account accountIn = await _context.Account.FindAsync(transactionDto.AccountInId);
        if (accountIn != null) {
          accountIn.Balance = _accountService.AdjustAccountBalance(accountIn.Balance, transactionDto.Amount, accountIn.AccountType, false);
        }
        _context.Entry(accountIn).State = EntityState.Modified;
      }
      if (transactionDto.AccountOutId != null)
      {
        Account accountOut = await _context.Account.FindAsync(transactionDto.AccountOutId);
        if (accountOut != null) {
          accountOut.Balance = _accountService.AdjustAccountBalance(accountOut.Balance, transactionDto.Amount, accountOut.AccountType, true);
        }
        _context.Entry(accountOut).State = EntityState.Modified;
      }
      await _context.SaveChangesAsync();

      // Refresh DTO.
      transactionDto = _mapper.Map<TransactionDto>(transaction);

      return CreatedAtAction("GetTransaction", new { Id = transaction.TransactionId }, transactionDto);
    }

    // PUT: api/Transactions/00000000-0000-0000-0000-000000000000
    [HttpPut("{id}")]
    public async Task<ActionResult> PutTransaction(Guid id, TransactionDto transactionDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      if (id != transactionDto.TransactionId)
      {
        return BadRequest("Transaction ID does not match the Posted object.");
      }

      Transaction transaction = await _context.Transaction.FindAsync(id);
      if (transaction == null)
      {
        return NotFound();
      }
      else if (transaction.UserId != userId)
      {
        return Unauthorized();
      }

      transaction.CategoryId = transactionDto.CategoryId;
      transaction.TransactionPartyId = transactionDto.TransactionPartyId;
      transaction.AccountInId = transactionDto.AccountInId;
      transaction.AccountOutId = transactionDto.AccountOutId;
      transaction.Amount = transactionDto.Amount;
      transaction.IsShared = transactionDto.IsShared;
      transaction.ModifiedOn = DateTime.Now;
      _context.Entry(transaction).State = EntityState.Modified;

      // Process balance change.
      if (transactionDto.AccountInId != null)
      {
        Account accountIn = await _context.Account.FindAsync(transactionDto.AccountInId);
        if (accountIn != null) {
          accountIn.Balance = _accountService.AdjustAccountBalance(accountIn.Balance, transactionDto.Amount, accountIn.AccountType, false);
        }
        _context.Entry(accountIn).State = EntityState.Modified;
      }
      if (transactionDto.AccountOutId != null)
      {
        Account accountOut = await _context.Account.FindAsync(transactionDto.AccountOutId);
        if (accountOut != null) {
          accountOut.Balance = _accountService.AdjustAccountBalance(accountOut.Balance, transactionDto.Amount, accountOut.AccountType, true);
        }
        _context.Entry(accountOut).State = EntityState.Modified;
      }
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/Transactions/00000000-0000-0000-0000-000000000000
    [HttpDelete("{id}")]
    public async Task<ActionResult<TransactionDto>> DeleteTransaction(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      Transaction transaction = await _context.Transaction.FindAsync(id);
      if (transaction == null)
      {
        return NotFound();
      }

      if (transaction.UserId != userId)
      {
        return Unauthorized();
      }

      _context.Remove(transaction);

      // Process balance change.
      if (transaction.AccountInId != null)
      {
        Account accountIn = await _context.Account.FindAsync(transaction.AccountInId);
        if (accountIn != null) {
          accountIn.Balance = _accountService.AdjustAccountBalance(accountIn.Balance, transaction.Amount, accountIn.AccountType, true);
        }
        _context.Entry(accountIn).State = EntityState.Modified;
      }
      if (transaction.AccountOutId != null)
      {
        Account accountOut = await _context.Account.FindAsync(transaction.AccountOutId);
        if (accountOut != null) {
          accountOut.Balance = _accountService.AdjustAccountBalance(accountOut.Balance, transaction.Amount, accountOut.AccountType, false);
        }
        _context.Entry(accountOut).State = EntityState.Modified;
      }

      await _context.SaveChangesAsync();

      TransactionDto transactionDto = _mapper.Map<TransactionDto>(transaction);
      return transactionDto;
    }
  }
}