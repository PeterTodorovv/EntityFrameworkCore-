using System;
using System.Collections.Generic;

namespace ORM_Fundamentals.models
{
    public partial class Part
    {
        public Part()
        {
            OrderParts = new HashSet<OrderPart>();
            PartsNeededs = new HashSet<PartsNeeded>();
        }

        public int PartId { get; set; }
        public string SerialNumber { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int VendorId { get; set; }
        public int StockQty { get; set; }

        public virtual Vendor Vendor { get; set; } = null!;
        public virtual ICollection<OrderPart> OrderParts { get; set; }
        public virtual ICollection<PartsNeeded> PartsNeededs { get; set; }
    }
}
