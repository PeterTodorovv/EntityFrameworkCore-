using System;
using System.Collections.Generic;

namespace ORM_Fundamentals.models
{
    public partial class Mechanic
    {
        public Mechanic()
        {
            Jobs = new HashSet<Job>();
        }

        public int MechanicId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;

        public virtual ICollection<Job> Jobs { get; set; }
    }
}
