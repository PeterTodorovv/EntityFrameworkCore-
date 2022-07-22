using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.DataProcessor.ImportDto
{
    public class ProjectionsImportDto
    {
        public ProjectionsImportDto()
        {
            tickets = new List<TicketImportDto>();
        }

        [Required]
        [MinLength(Constants.TheatreNameMinLength)]
        [MaxLength(Constants.TheatreNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [Range(Constants.MinNumberOfHalls, Constants.MaxNumberOfHalls)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MinLength(Constants.DirectorMinLength)]
        [MaxLength(Constants.DirectorMaxLength)]
        public string Director { get; set; }

        public List<TicketImportDto> tickets { get; set; }

    }
}
