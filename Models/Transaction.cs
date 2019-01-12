using System;
using System.Collections.Generic;

namespace SplitApi.Models
{
    public partial class Transaction
    {
        public Guid Id { get; set; }
        public Guid? Category { get; set; }
        public Guid? TransactionParty { get; set; }
        public decimal Amount { get; set; }
        public bool IsShared { get; set; }
        public DateTime Date { get; set; }
        public Guid? AccountOut { get; set; }
        public Guid? AccountIn { get; set; }

        public Account AccountInNavigation { get; set; }
        public Account AccountOutNavigation { get; set; }
        public Category CategoryNavigation { get; set; }
        public TransactionParty TransactionPartyNavigation { get; set; }
    }
}
