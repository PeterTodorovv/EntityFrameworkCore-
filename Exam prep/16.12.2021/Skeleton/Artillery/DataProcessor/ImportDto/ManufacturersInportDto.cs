using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Manufacturer")]
    public class ManufacturersInportDto
    {
        [XmlElement("ManufacturerName")]
        [Required]
        [MinLength(Constants.MANIFACTURER_NAME_MIN_LENGTH)]
        [MaxLength(Constants.MANIFACTURER_NAME_MAX_LENGTH)]
        public string ManufacturerName { get; set; }

        [XmlElement("Founded")]
        [Required]
        [MinLength(Constants.FOUNDED_MIN_LENGTH)]
        [MaxLength(Constants.FOUNDED_MAX_LENGTH)]
        public string Founded { get; set; }
    }
}
