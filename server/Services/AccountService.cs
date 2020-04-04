using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Split.Models;
using Split.Dtos;

namespace Split.Services
{
  public interface IAccountService
  {
      decimal AdjustAccountBalance(decimal? currentBalance, decimal? transactionAmt, int accountType, bool isExpense);
  }

  public class AccountService : IAccountService
  {
    private SplitContext _context;

    public AccountService(SplitContext context)
    {
      _context = context;
    }

    public decimal AdjustAccountBalance(decimal? currentBalance, decimal? transactionAmt, int accountType, bool isExpense)
    {
      if (!currentBalance.HasValue || !transactionAmt.HasValue) return currentBalance ?? 0;
      
      if (accountType == (int)AccountType.Credit)
      {
        return isExpense ? currentBalance.Value + transactionAmt.Value : currentBalance.Value - transactionAmt.Value;
      }
      else
      {
        return isExpense ? currentBalance.Value - transactionAmt.Value : currentBalance.Value + transactionAmt.Value;
      }
    }
  }
}