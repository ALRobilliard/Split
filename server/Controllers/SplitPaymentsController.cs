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
using Split.Dtos;
using Split.Extensions;
using Split.Helpers;
using Split.Models;

namespace Split.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class SplitPaymentsController : ControllerBase
  {
    private IMapper _mapper;
    private readonly SplitContext _context;

    public SplitPaymentsController(IMapper mapper, SplitContext context)
    {
      _mapper = mapper;
      _context = context;
    }

    // GET: api/SplitPayments/00000000-0000-0000-0000-000000000000
    [HttpGet("{id}")]
    public async Task<ActionResult<SplitPaymentDto>> GetSplitPayment(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from Token");
      }

      SplitPayment splitPayment = await _context.SplitPayment.FindAsync(id);
      Transaction transaction = await _context.Transaction.FindAsync(splitPayment.TransactionId);

      if (splitPayment == null)
      {
        return NotFound();
      }

      if (transaction.UserId != userId)
      {
        return Unauthorized();
      }

      SplitPaymentDto splitPaymentDto = _mapper.Map<SplitPaymentDto>(splitPayment);
      return splitPaymentDto;
    }

    // POST: api/SplitPayments/GetPaymentsForTransaction
    [HttpPost("GetPaymentsForTransaction")]
    public async Task<ActionResult<List<SplitPaymentDto>>> GetSplitPayments([FromBody] Guid transactionId)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      Transaction transaction = await _context.Transaction.FindAsync(transactionId);
      if (transaction == null)
      {
        return NotFound();
      }

      if (transaction.UserId != userId)
      {
        return Unauthorized();
      }

      List<SplitPayment> splitPayments = await _context.SplitPayment.Where(
        sp => sp.TransactionId.Equals(transaction.TransactionId)
      ).ToListAsync();

      List<SplitPaymentDto> splitPaymentDtos = _mapper.Map<List<SplitPaymentDto>>(splitPayments);
      return splitPaymentDtos;
    }

    // POST: api/SplitPayments
    [HttpPost]
    public async Task<ActionResult<SplitPaymentDto>> PostSplitPayment(SplitPaymentDto splitPaymentDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      Transaction transaction = await _context.Transaction.FindAsync(splitPaymentDto.TransactionId);
      if (transaction.UserId != userId)
      {
        return Unauthorized();
      }

      SplitPayment splitPayment = _mapper.Map<SplitPayment>(splitPaymentDto);

      _context.SplitPayment.Add(splitPayment);
      await _context.SaveChangesAsync();

      // Refres DTO.
      splitPaymentDto = _mapper.Map<SplitPaymentDto>(splitPayment);

      return CreatedAtAction("GetSplitPayment", new { Id = splitPayment.SplitPaymentId }, splitPaymentDto);
    }

    // PUT: api/SplitPayments/00000000-0000-0000-0000-000000000000
    [HttpPut("{id}")]
    public async Task<ActionResult> PutSplitPayment(Guid id, SplitPaymentDto splitPaymentDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      if (id != splitPaymentDto.SplitPaymentId)
      {
        return BadRequest("Split Payment ID does not match the Posted object.");
      }

      Transaction transaction = await _context.Transaction.FindAsync(splitPaymentDto.TransactionId);
      if (transaction.UserId != userId)
      {
        return Unauthorized();
      }

      SplitPayment splitPayment = await _context.SplitPayment.FindAsync(id);
      if (splitPayment == null)
      {
        return NotFound();
      }

      splitPayment.Amount = splitPaymentDto.Amount;
      splitPayment.PayeeId = splitPaymentDto.PayeeId;
      splitPayment.ModifiedOn = DateTime.Now;
      _context.Entry(splitPayment).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/SplitPayments/00000000-0000-0000-0000-000000000000
    [HttpDelete("{id}")]
    public async Task<ActionResult<SplitPaymentDto>> DeleteSplitPayment(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be read from token.");
      }

      SplitPayment splitPayment = await _context.SplitPayment.FindAsync(id);
      if (splitPayment == null)
      {
        return NotFound();
      }

      Transaction transaction = await _context.Transaction.FindAsync(splitPayment.TransactionId);
      if (transaction == null)
      {
        return NotFound("Parent Transaction not found.");
      }
      else if (transaction.UserId != userId)
      {
        return Unauthorized();
      }

      _context.Remove(splitPayment);
      await _context.SaveChangesAsync();

      SplitPaymentDto splitPaymentDto = _mapper.Map<SplitPaymentDto>(splitPayment);
      return splitPaymentDto;
    }
  }
}