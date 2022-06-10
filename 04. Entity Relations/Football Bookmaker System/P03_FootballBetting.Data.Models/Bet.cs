using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    internal class Bet
    {
        //BetId, Amount, Prediction, DateTime, UserId, GameId
        [Key]
        public int BetId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int Prediction { get ; set; }
        [Required]
        public DateTime? DateTime { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int GameId { get; set; }
    }
}
