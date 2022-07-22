using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        public Play()
        {
            Casts = new List<Cast>();
            Tickets = new List<Ticket>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Constants.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public TimeSpan Duration  { get; set; }

        [Required]
        [Range(Constants.MinRating, Constants.MaxRating)]
        public double Rating { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        [MaxLength(Constants.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(Constants.ScreenWriterMaxLength)]
        public string Screenwriter { get; set; }

        public virtual List<Cast> Casts { get; set; }
        public virtual List<Ticket> Tickets { get; set; }
    }
}
