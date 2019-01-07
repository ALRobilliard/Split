using System;
using System.Collections.Generic;

namespace split_api.Models
{
    public partial class TransactionParty
    {
        public TransactionParty()
        {
            Transactions = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? DefaultCategory { get; set; }

        public Category DefaultCategoryNavigation { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
