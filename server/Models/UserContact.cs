using System;
using System.Collections.Generic;

namespace Split.Models
{
    public partial class UserContact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ContactId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public User Contact { get; set; }
        public User User { get; set; }
    }
}
