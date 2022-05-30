using System;
using System.Collections.Generic;

namespace ORM_Fundamentals.models
{
    public partial class Client
    {
        public Client()
        {
            Jobs = new HashSet<Job>();
        }

        public int ClientId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public virtual ICollection<Job> Jobs { get; set; }
    }
}
