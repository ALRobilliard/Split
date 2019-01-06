using System;
using System.Collections.Generic;

namespace split_api.Models
{
    public partial class Transactions
    {
        public Guid Id { get; set; }
        public Guid? Category { get; set; }
        public Guid? TransactionParty { get; set; }
        public decimal Amount { get; set; }
        public bool IsShared { get; set; }
        public DateTime Date { get; set; }
        public Guid? AccountOut { get; set; }
        public Guid? AccountIn { get; set; }

        public Accounts AccountInNavigation { get; set; }
        public Accounts AccountOutNavigation { get; set; }
        public Categories CategoryNavigation { get; set; }
        public TransactionParties TransactionPartyNavigation { get; set; }
    }
}
