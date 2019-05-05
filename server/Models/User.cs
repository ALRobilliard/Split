using System;
using System.Collections.Generic;

namespace Split.Models
{
    public partial class User
    {
        public User()
        {
            Account = new HashSet<Account>();
            Category = new HashSet<Category>();
            SplitPayment = new HashSet<SplitPayment>();
            Transaction = new HashSet<Transaction>();
            TransactionParty = new HashSet<TransactionParty>();
            UserContactContact = new HashSet<UserContact>();
            UserContactUser = new HashSet<UserContact>();
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? IsRegistered { get; set; }
        public bool? ConfirmedEmail { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? LastLogin { get; set; }

        public ICollection<Account> Account { get; set; }
        public ICollection<Category> Category { get; set; }
        public ICollection<SplitPayment> SplitPayment { get; set; }
        public ICollection<Transaction> Transaction { get; set; }
        public ICollection<TransactionParty> TransactionParty { get; set; }
        public ICollection<UserContact> UserContactContact { get; set; }
        public ICollection<UserContact> UserContactUser { get; set; }
    }
}
