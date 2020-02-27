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
  public class UserContactsController : ControllerBase
  {
    private IMapper _mapper;
    private readonly SplitContext _context;

    public UserContactsController(IMapper mapper, SplitContext context)
    {
      _mapper = mapper;
      _context = context;
    }

    // GET: api/UserContacts/00000000-0000-0000-0000-000000000000
    [HttpGet("{id}")]
    public async Task<ActionResult<UserContactDto>> GetUserContact(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from Token");
      }

      UserContact userContact = await _context.UserContact.FindAsync(id);
      if (userContact == null)
      {
        return NotFound("UserContact not found.");
      }

      User contact = await _context.User.FindAsync(userContact.ContactId);
      if (contact == null)
      {
        return NotFound("UserContact Contact not found.");
      }

      UserContactDto userContactDto = _mapper.Map<UserContactDto>(userContact);
      userContactDto.ContactName = contact.FirstName + " " + contact.LastName;
      userContactDto.ContactEmail = contact.Email;

      return userContactDto;
    }

    // GET: api/UserContacts
    [HttpGet]
    public async Task<ActionResult<List<UserContactDto>>> GetUserContacts()
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from Token");
      }

      List<UserContact> userContacts = await _context.UserContact.ToListAsync();
      List<UserContactDto> userContactDtos = _mapper.Map<List<UserContactDto>>(userContacts);

      // Get linked Contact details before returning list.
      foreach(var userContactDto in userContactDtos)
      {
        User contact = await _context.User.FindAsync(userContactDto.UserId);
        if (contact != null)
        {
          if (contact.FirstName != null || contact.LastName != null)
          {
            userContactDto.ContactName = contact.FirstName + " " + contact.LastName;
          }

          if (contact.Email != null)
          {
            userContactDto.ContactEmail = contact.Email;
          }
        }
      }

      return userContactDtos;
    }

    // POST: api/UserContacts
    [HttpPost]
    public async Task<ActionResult<UserContactDto>> PostUserContact(UserContactDto userContactDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      userContactDto.UserId = userId.Value;

      if (userContactDto.ContactId == null)
      {
        return BadRequest("UserContact creation requires a ContactId");
      }

      User contact = await _context.User.FindAsync(userContactDto.UserId);
      if (contact == null)
      {
        return NotFound("UserContact Contact not found");
      }

      UserContact userContact = _mapper.Map<UserContact>(userContactDto);

      _context.UserContact.Add(userContact);
      await _context.SaveChangesAsync();

      // Refresh DTO.
      userContactDto = _mapper.Map<UserContactDto>(userContact);

      return CreatedAtAction("GetUserContact", new { Id = userContact.Id }, userContactDto);
    }

    // PUT: api/UserContacts/00000000-0000-0000-0000-000000000000
    [HttpPut("{id}")]
    public async Task<ActionResult> PutUserContact(Guid id, UserContactDto userContactDto)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be retrieved from token.");
      }

      if (userId != userContactDto.UserId)
      {
        return Unauthorized();
      }

      if (id != userContactDto.Id)
      {
        return BadRequest("UserContact ID does not match the Posted object.");
      }

      UserContact userContact = await _context.UserContact.FindAsync(id);
      if (userContact == null)
      {
        return NotFound();
      }

      userContact.Name = userContactDto.Name;
      userContact.ContactId = userContactDto.ContactId;
      userContact.ModifiedOn = DateTime.Now;
      _context.Entry(userContact).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/UserContact/00000000-0000-0000-0000-000000000000
    [HttpDelete("{id}")]
    public async Task<ActionResult<UserContactDto>> DeleteUserContact(Guid id)
    {
      ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
      Guid? userId = identity.GetUserId();
      if (userId == null)
      {
        return BadRequest("User ID unable to be read from token.");
      }

      UserContact userContact = await _context.UserContact.FindAsync(id);
      if (userContact == null)
      {
        return NotFound();
      }

      if (userContact.UserId != userId)
      {
        return Unauthorized();
      }

      _context.Remove(userContact);
      await _context.SaveChangesAsync();

      UserContactDto userContactDto = _mapper.Map<UserContactDto>(userContact);
      return userContactDto;
    }
  }
}