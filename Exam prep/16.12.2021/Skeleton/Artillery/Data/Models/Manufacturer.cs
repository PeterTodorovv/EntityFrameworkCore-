using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Artillery.Data.Models
{
    public class Manufacturer
    {
        public Manufacturer()
        {
            Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(Constants.MANIFACTURER_NAME_MAX_LENGTH)]
        public string ManufacturerName { get; set; }

        [Required]
        [StringLength(Constants.FOUNDED_MAX_LENGTH)]
        public string Founded { get; set; }

        public virtual ICollection<Gun> Guns { get; set; }
    }
}
