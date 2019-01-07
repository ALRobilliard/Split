using System;
using System.Collections.Generic;

namespace split_api.Models
{
    public partial class Account
    {
        public Account()
        {
            TransactionAccountInNavigation = new HashSet<Transaction>();
            TransactionAccountOutNavigation = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Transaction> TransactionAccountInNavigation { get; set; }
        public ICollection<Transaction> TransactionAccountOutNavigation { get; set; }
    }
}
