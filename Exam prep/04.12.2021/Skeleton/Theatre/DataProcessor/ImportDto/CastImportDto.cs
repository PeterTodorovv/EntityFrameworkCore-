using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class CastImportDto
    {
        [Required]
        [XmlElement("FullName")]
        [MinLength(Constants.CastNameMinLength)]
        [MaxLength(Constants.CastNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        [XmlElement("IsMainCharacter")]
        public string IsMainCharacter { get; set; }

        [Required]
        [XmlElement("PhoneNumber")]
        [RegularExpression(Constants.PhoneNumberRegex)]
        public string PhoneNumber { get; set; }

        [Required]
        [XmlElement("PlayId")]
        public int PlayId { get; set; }
    }
}
