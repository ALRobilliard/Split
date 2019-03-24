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
  public class TransactionPartiesController : ControllerBase
  {
    private IMapper _mapper;
    private readonly SplitContext _context;

    public TransactionPartiesController(IMapper mapper, SplitContext context)
    {
      _mapper = mapper;
      _context = context;
    }

    // GET: api/TransactionParties
    [HttpGet]
    public async Task<ActionResult<List<TransactionPartyDto>>> GetAll()
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      List<TransactionParty> transactionParties = await _context.TransactionParty.Where(
        tp => tp.UserId.Equals(userId) || tp.UserId.Equals(null)
      ).ToListAsync();
      return _mapper.Map<List<TransactionPartyDto>>(transactionParties);
    }

    // GET: api/TransactionParties/00000000-0000-0000-0000-000000000000
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionPartyDto>> GetTransaction(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      TransactionParty transactionParty = await _context.TransactionParty.FindAsync(id);
      if (transactionParty == null)
      {
        return NotFound();
      }

      if (transactionParty.UserId != null && transactionParty.UserId != userId)
      {
        return Unauthorized();
      }

      return _mapper.Map<TransactionPartyDto>(transactionParty);
    }

    // POST: api/TransactionParties/search
    [HttpPost("search")]
    public async Task<ActionResult<List<TransactionPartyDto>>> GetByName([FromBody] string transactionPartyName)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      List<TransactionParty> transactionParties = await _context.TransactionParty.Where(
        tp =>
          tp.TransactionPartyName.StartsWith(transactionPartyName) &&
          (
            tp.UserId.Equals(userId.Value) ||
            tp.UserId.Equals(null)
          )
        ).ToListAsync();
      return _mapper.Map<List<TransactionPartyDto>>(transactionParties);
    }


    // POST: api/TransactionParties
    [HttpPost]
    public async Task<ActionResult<TransactionPartyDto>> PostTransactionParty(TransactionPartyDto transactionPartyDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      TransactionParty transactionParty = _mapper.Map<TransactionParty>(transactionPartyDto);
      transactionParty.UserId = userId;
      _context.TransactionParty.Add(transactionParty);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetCategory", new { Id = transactionParty.TransactionPartyId }, transactionPartyDto);
    }

    // PUT: api/TransactionParties/00000000-0000-0000-0000-000000000000
    [HttpPut("{id}")]
    public async Task<ActionResult> PutTransactionParty(Guid id, TransactionPartyDto transactionPartyDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      if (id != transactionPartyDto.TransactionPartyId)
      {
        return BadRequest();
      }

      if (transactionPartyDto.UserId != null && transactionPartyDto.UserId != userId)
      {
        return Unauthorized();
      }

      TransactionParty transactionParty = _mapper.Map<TransactionParty>(transactionPartyDto);

      _context.Entry(transactionParty).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/TransactionParties/00000000-0000-0000-0000-000000000000
    [HttpDelete("{id}")]
    public async Task<ActionResult<TransactionPartyDto>> DeleteTransactionParty(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();

      TransactionParty transactionParty = await _context.TransactionParty.FindAsync(id);
      if (transactionParty == null)
      {
        return NotFound();
      }

      if (transactionParty.UserId != null && transactionParty.UserId != userId)
      {
        return Unauthorized();
      }

      _context.Remove(transactionParty);
      await _context.SaveChangesAsync();

      TransactionPartyDto transactionPartyDto = _mapper.Map<TransactionPartyDto>(transactionParty);

      return transactionPartyDto;
    }
  }
}