using System;
using System.Collections.Generic;

namespace split_api.Models
{
    public partial class Accounts
    {
        public Accounts()
        {
            TransactionsAccountInNavigation = new HashSet<Transactions>();
            TransactionsAccountOutNavigation = new HashSet<Transactions>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Transactions> TransactionsAccountInNavigation { get; set; }
        public ICollection<Transactions> TransactionsAccountOutNavigation { get; set; }
    }
}
