using System;
using System.Collections.Generic;

namespace ORM_Fundamentals.models
{
    public partial class Job
    {
        public Job()
        {
            Orders = new HashSet<Order>();
            PartsNeededs = new HashSet<PartsNeeded>();
        }

        public int JobId { get; set; }
        public int ModelId { get; set; }
        public string Status { get; set; } = null!;
        public int ClientId { get; set; }
        public int MechanicId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? FinishDate { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual Mechanic Mechanic { get; set; } = null!;
        public virtual Model Model { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PartsNeeded> PartsNeededs { get; set; }
    }
}
