using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class Town
    {
        [Key]
        public int TownId { get; set; }
        [Required]
        [MaxLength(50)]
        public string TownName { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}
