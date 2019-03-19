using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
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
      List<TransactionParty> transactionParties = await _context.TransactionParty.ToListAsync();
      return _mapper.Map<List<TransactionPartyDto>>(transactionParties);
    }

    // POST: api/TransactionParties/search
    [HttpPost("search")]
    public async Task<ActionResult<List<TransactionPartyDto>>> GetByName([FromBody] string transactionPartyName)
    {
      List<TransactionParty> transactionParties = await _context.TransactionParty.Where(
        tp => tp.TransactionPartyName.StartsWith(transactionPartyName)
        ).ToListAsync();
      return _mapper.Map<List<TransactionPartyDto>>(transactionParties);
    }


    // POST: api/TransactionParties
    [HttpPost]
    public async Task<ActionResult<TransactionPartyDto>> PostTransactionParty(TransactionPartyDto transactionPartyDto)
    {
      TransactionParty transactionParty = _mapper.Map<TransactionParty>(transactionPartyDto);
      _context.TransactionParty.Add(transactionParty);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetCategory", new { Id = transactionParty.TransactionPartyId }, transactionPartyDto);
    }
  }
}