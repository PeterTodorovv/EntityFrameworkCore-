using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    internal class Game
    {
        //GameId, HomeTeamId, AwayTeamId, HomeTeamGoals, AwayTeamGoals, DateTime, HomeTeamBetRate, AwayTeamBetRate, DrawBetRate, Result)

        [Key]
        public int GameId { get; set; }
        [Required]
        public int HomeTeamId { get; set; }
        [Required]
        public int AwayTeamId { get; set; }

        [Required]
        public byte HomeTeamGoals { get; set; }

        [Required]
        public byte AwayTeamGoals { get; set; }
        [Required]

        public DateTime? DateTime { get; set; }
        [Required]
        public int HomeTeamBetRate { get; set; }
        [Required]
        public int AwayTeamBetRate { get; set; }
        [Required]
        public int DrawBetRate { get; set; }
        [Required]
        public int Result { get; set; }
    }
}
