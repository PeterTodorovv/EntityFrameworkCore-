using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    internal class Country
    {
        [Key]
        public int CountryId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
