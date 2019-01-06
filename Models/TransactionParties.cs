using System;
using System.Collections.Generic;

namespace split_api.Models
{
    public partial class TransactionParties
    {
        public TransactionParties()
        {
            Transactions = new HashSet<Transactions>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? DefaultCategory { get; set; }

        public Categories DefaultCategoryNavigation { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
    }
}
