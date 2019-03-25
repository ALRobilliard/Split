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
  [Authorize]
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
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be read from token.");
      }

      List<Transaction> transactions = await _context.Transaction.Where(
        t => t.UserId.Equals(userId) &&
        t.TransactionDate.CompareTo(startDate) >= 0 &&
        t.TransactionDate.CompareTo(endDate) <= 0
        ).ToListAsync();

      List<TransactionDto> transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);
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
      if (id != transactionDto.TransactionId)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      if (id != transactionDto.UserId)
      {
        return BadRequest("Transaction ID does not match the Posted object.");
      }

      if (transactionDto.UserId != userId)
      {
        return Unauthorized();
      }

      Transaction transaction = await _context.Transaction.FindAsync(id);
      if (transaction == null)
      {
        return NotFound();
      }

      transaction.CategoryId = transactionDto.CategoryId;
      transaction.TransactionPartyId = transactionDto.TransactionPartyId;
      transaction.AccountInId = transactionDto.AccountInId;
      transaction.AccountOutId = transactionDto.AccountOutId;
      transaction.Amount = transactionDto.Amount;
      transaction.IsShared = transactionDto.IsShared;
      transaction.ModifiedOn = DateTime.Now;
      _context.Entry(transaction).State = EntityState.Modified;
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
      await _context.SaveChangesAsync();

      TransactionDto transactionDto = _mapper.Map<TransactionDto>(transaction);
      return transactionDto;
    }
  }
}