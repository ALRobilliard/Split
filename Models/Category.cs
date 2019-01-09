using System;
using System.Collections.Generic;

namespace SplitApi.Models
{
    public partial class Category
    {
        public Category()
        {
            TransactionParties = new HashSet<TransactionParty>();
            Transactions = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CategoryType { get; set; }

        public ICollection<TransactionParty> TransactionParties { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
