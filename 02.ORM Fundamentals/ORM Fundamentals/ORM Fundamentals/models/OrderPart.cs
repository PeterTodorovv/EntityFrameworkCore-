using System;
using System.Collections.Generic;

namespace ORM_Fundamentals.models
{
    public partial class OrderPart
    {
        public int OrderId { get; set; }
        public int PartId { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Part Part { get; set; } = null!;
    }
}
