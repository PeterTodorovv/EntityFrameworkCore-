using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    internal class Position
    {
        [Key]
        public int PositionId { get; set; }
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
    }
}
