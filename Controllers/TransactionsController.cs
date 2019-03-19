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
  [Route("api/[controller]")]
  [ApiController]
  public class TransactionsController : ControllerBase
  {
    private IMapper _mapper;
    private readonly SplitContext _context;

    public TransactionsController(IMapper mapper, SplitContext context)
    {
      _mapper = mapper;
      _context = context;
    }

    // GET: api/Transcations
    [HttpGet]
    public async Task<ActionResult<List<TransactionDto>>> GetTransactions(DateTime startDate, DateTime endDate)
    {
      List<Transaction> transactions = null;
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      if (userId != null)
      {
        transactions = await _context.Transaction.Where(
          t => t.UserId.Equals(userId) &&
          t.TransactionDate.CompareTo(startDate) >= 0 &&
          t.TransactionDate.CompareTo(endDate) <= 0
          ).ToListAsync();
      }

      return _mapper.Map<List<TransactionDto>>(transactions);
    }

    // GET: api/Transactions/00000000-0000-0000-0000-000000000000
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetTransaction(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      Transaction transaction = await _context.Transaction.FindAsync(id);
      if (transaction == null)
      {
        return NotFound();
      }

      if (transaction.UserId != userId)
      {
        return Unauthorized();
      }

      return _mapper.Map<TransactionDto>(transaction);
    }

    // POST: api/Transactions
    [HttpPost]
    public async Task<ActionResult<TransactionDto>> PostTransaction(Transaction transaction)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      transaction.UserId = userId;

      _context.Transaction.Add(transaction);
      await _context.SaveChangesAsync();

      TransactionDto transactionDto = _mapper.Map<TransactionDto>(transaction);
      return CreatedAtAction("GetTransaction", new { Id = transaction.TransactionId }, transactionDto);
    }

    // PUT: api/Transactions/00000000-0000-0000-0000-000000000000
    [HttpPut("{id}")]
    public async Task<ActionResult> PutTransaction(Guid id, Transaction transaction)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      if (id != transaction.TransactionId)
      {
        return BadRequest();
      }

      if (transaction.UserId != userId)
      {
        return Unauthorized();
      }

      _context.Entry(transaction).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/Transactions/00000000-0000-0000-0000-000000000000
    [HttpDelete("{id}")]
    public async Task<ActionResult<Transaction>> DeleteTransaction(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

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
      await _context.SaveChangesAsync();

      return transaction;
    }
  }
}