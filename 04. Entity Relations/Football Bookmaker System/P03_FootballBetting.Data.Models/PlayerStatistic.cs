using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    internal class PlayerStatistic
    {
        [Required]
        public int GameId { get; set; }
        [Required]
        public int PlayerId { get; set; }
        [Required]
        public byte ScoredGoals { get; set; }
        [Required]
        public byte Assists { get; set; }
        [Required]
        public int MinutesPlayed { get; set; }
    }
}
