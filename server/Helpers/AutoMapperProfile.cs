using AutoMapper;
using Split.Dtos;
using Split.Models;

namespace Split.Helpers
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<Account, AccountDto>();
      CreateMap<AccountDto, Account>();
      CreateMap<Category, CategoryDto>();
      CreateMap<CategoryDto, Category>();
      CreateMap<SplitPayment, SplitPaymentDto>();
      CreateMap<SplitPaymentDto, SplitPayment>();
      CreateMap<Transaction, TransactionDto>();
      CreateMap<TransactionDto, Transaction>();
      CreateMap<TransactionParty, TransactionPartyDto>();
      CreateMap<TransactionPartyDto, TransactionParty>();
      CreateMap<User, UserDto>();
      CreateMap<UserDto, User>();
      CreateMap<UserContact, UserContactDto>();
      CreateMap<UserContactDto, UserContact>();
    }
  }
}