using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    public class Team
    {
        //TeamId, Name, LogoUrl, Initials (JUV, LIV, ARS…), Budget, PrimaryKitColorId, SecondaryKitColorId, TownId
        [Key]
        public int TeamId { get; set; }

        [MaxLength(2048)]
        public string LogoUrl { get; set; }

        [Required]
        [MaxLength(3)]
        public string Initials { get; set; }

        public decimal Budget { get; set; }

        [Required]
        public int PrimaryKitColorId { get; set; }

        [Required]
        public int SecondaryKitColorId { get; set; }
        [Required]
        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public Town Town { get; set; }
    }
}
