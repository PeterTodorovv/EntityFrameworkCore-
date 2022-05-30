using System;
using System.Collections.Generic;

namespace ORM_Fundamentals.models
{
    public partial class PartsNeeded
    {
        public int JobId { get; set; }
        public int PartId { get; set; }
        public int Quantity { get; set; }

        public virtual Job Job { get; set; } = null!;
        public virtual Part Part { get; set; } = null!;
    }
}
