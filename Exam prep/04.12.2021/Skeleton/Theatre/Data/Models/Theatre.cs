using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.Data.Models
{
    public class Theatre
    {
        public Theatre()
        {
            Tickets = new List<Ticket>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Constants.TheatreNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [Range(Constants.MinNumberOfHalls, Constants.MaxNumberOfHalls)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MaxLength(Constants.DirectorMaxLength)]
        public string Director  { get; set; }

        public virtual List<Ticket> Tickets { get; set; }
    }
}
