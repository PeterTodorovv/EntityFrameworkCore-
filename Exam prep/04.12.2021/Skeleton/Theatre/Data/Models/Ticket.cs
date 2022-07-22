﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(Constants.MinPrice, Constants.MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [Range(Constants.MinRowNumber, Constants.MaxRowNumber)]
        public int RowNumberr { get; set; }

        [Required]
        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }
        public virtual Play Play { get; set; }

        [Required]
        [ForeignKey(nameof(Theatre))]
        public int TheatreId  { get; set; }
        public Theatre Theatre { get; set; }
    }
}
