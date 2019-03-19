using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using SplitApi.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SplitApi.Services;
using SplitApi.Dtos;
using SplitApi.Models;

namespace SplitApi.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class UsersController : ControllerBase
  {
    private IUserService _userService;
    private IMapper _mapper;
    private readonly AppSettings _appSettings;

    public UsersController(
        IUserService userService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _userService = userService;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    // POST: api/Users/authenticate
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody]UserDto userDto)
    {
      var user = _userService.Authenticate(userDto.Username, userDto.Password);

      if (user == null)
        return BadRequest(new { message = "Username or password is incorrect" });

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
          }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);

      // return basic user info (without password) and token to store client side
      return Ok(new
      {
        Id = user.UserId,
        Username = user.Username,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Token = tokenString
      });
    }

    // POST: api/Users/register
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody]UserDto userDto)
    {
      // map dto to entity
      var user = _mapper.Map<User>(userDto);

      try
      {
        // save 
        _userService.Create(user, userDto.Password);
        return Ok();
      }
      catch (AppException ex)
      {
        // return error message if there was an exception
        return BadRequest(new { message = ex.Message });
      }
    }

    // GET: api/Users
    [HttpGet]
    public IActionResult GetUsers()
    {
      var users = _userService.GetAll();
      var userDtos = _mapper.Map<IList<UserDto>>(users);
      return Ok(userDtos);
    }

    // GET: api/Users/00000000-0000-0000-0000-000000000000
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
      var user = _userService.GetById(id);
      var userDto = _mapper.Map<UserDto>(user);
      return Ok(userDto);
    }

    // PUT: api/Users/00000000-0000-0000-0000-000000000000
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody]UserDto userDto)
    {
      // map dto to entity and set id
      var user = _mapper.Map<User>(userDto);
      user.UserId = id;

      try
      {
        // save 
        _userService.Update(user, userDto.Password);
        return Ok();
      }
      catch (AppException ex)
      {
        // return error message if there was an exception
        return BadRequest(new { message = ex.Message });
      }
    }

    // DELETE: api/Users/00000000-0000-0000-0000-000000000000
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
      _userService.Delete(id);
      return Ok();
    }
  }
}