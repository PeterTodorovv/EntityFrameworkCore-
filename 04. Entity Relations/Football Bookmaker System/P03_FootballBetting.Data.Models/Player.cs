using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    internal class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
