using System;
using System.Collections.Generic;

namespace split_api.Models
{
    public partial class Categories
    {
        public Categories()
        {
            TransactionParties = new HashSet<TransactionParties>();
            Transactions = new HashSet<Transactions>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CategoryType { get; set; }

        public ICollection<TransactionParties> TransactionParties { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
    }
}
