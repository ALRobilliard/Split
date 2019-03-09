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
  public class TransactionsController : ControllerBase
  {
    private readonly SplitContext _context;

    public TransactionsController(SplitContext context)
    {
      _context = context;
    }

    // GET: api/Transcations
    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetTransactions()
    {
      return await _context.Transactions.ToListAsync();
    }

    // GET: api/Transactions/00000000-0000-0000-0000-000000000000
    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
    {
      Transaction transaction = await _context.Transactions.FindAsync(id);
      if (transaction == null)
      {
        return NotFound();
      }

      return transaction;
    }

    // POST: api/Transactions
    [HttpPost]
    public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
    {
      _context.Transactions.Add(transaction);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTransaction", new { Id = transaction.TransactionId }, transaction);
    }

    // PUT: api/Transactions/00000000-0000-0000-0000-000000000000
    [HttpPut("{id}")]
    public async Task<ActionResult> PutTransaction(Guid id, Transaction transaction)
    {
      if (id != transaction.TransactionId)
      {
        return BadRequest();
      }

      _context.Entry(transaction).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/Transactions/00000000-0000-0000-0000-000000000000
    [HttpDelete("{id}")]
    public async Task<ActionResult<Transaction>> DeleteTransaction(Guid id)
    {
      Transaction transaction = await _context.Transactions.FindAsync(id);
      if (transaction == null)
      {
        return NotFound();
      }

      _context.Remove(transaction);
      await _context.SaveChangesAsync();

      return transaction;
    }
  }
}