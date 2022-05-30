using System;
using System.Collections.Generic;

namespace ORM_Fundamentals.models
{
    public partial class Order
    {
        public Order()
        {
            OrderParts = new HashSet<OrderPart>();
        }

        public int OrderId { get; set; }
        public int JobId { get; set; }
        public DateTime? IssueDate { get; set; }
        public bool Delivered { get; set; }

        public virtual Job Job { get; set; } = null!;
        public virtual ICollection<OrderPart> OrderParts { get; set; }
    }
}
