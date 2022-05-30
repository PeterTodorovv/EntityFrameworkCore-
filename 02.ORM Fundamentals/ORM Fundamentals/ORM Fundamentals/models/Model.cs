using System;
using System.Collections.Generic;

namespace ORM_Fundamentals.models
{
    public partial class Model
    {
        public Model()
        {
            Jobs = new HashSet<Job>();
        }

        public int ModelId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Job> Jobs { get; set; }
    }
}
