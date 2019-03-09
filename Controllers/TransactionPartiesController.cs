using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using SplitApi.Models;

namespace SplitApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TransactionPartiesController : ControllerBase
  {
    private readonly SplitContext _context;

    public TransactionPartiesController(SplitContext context)
    {
      _context = context;
    }

    [HttpGet]
    public ActionResult<List<TransactionParty>> GetAll()
    {
      return _context.TransactionParty.ToList();
    }
  }
}