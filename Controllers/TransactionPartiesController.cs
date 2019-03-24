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
  }
}