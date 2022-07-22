using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class PlayImportDto
    {
        [XmlElement("Title")]
        [MinLength(Constants.TitleMinLength)]
        [MaxLength(Constants.TitleMaxLength)]
        [Required]
        public string Title { get; set; }

        [XmlElement("Duration")]
        [Required]
        public string Duration { get; set; }

        [XmlElement("Rating")]
        [Range(Constants.MinRating, Constants.MaxRating)]
        [Required]
        public double Rating { get; set; }

        [XmlElement("Genre")]
        [Required]
        public string Genre { get; set; }

        [XmlElement("Description")]
        [MaxLength(Constants.DescriptionMaxLength)]
        [Required]
        public string Description { get; set; }

        [XmlElement("Screenwriter")]
        [MinLength(Constants.ScreenWriterMinLength)]
        [MaxLength(Constants.ScreenWriterMaxLength)]
        [Required]
        public string Screenwriter { get; set; }
    }
}
