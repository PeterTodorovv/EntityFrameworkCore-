using System;
using System.Collections.Generic;

namespace ORM_Fundamentals.models
{
    public partial class Vendor
    {
        public Vendor()
        {
            Parts = new HashSet<Part>();
        }

        public int VendorId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Part> Parts { get; set; }
    }
}
