using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Shell")]
    public class ShellsImportDto
    {
        [Required]
        [XmlElement("ShellWeight")]
        public string ShellWeight { get; set; }
        
        [Required]
        [XmlElement("Caliber")]
        [MinLength(Constants.CALIBER_MIN_LENGTH)]
        [MaxLength(Constants.CALIBER_MAX_LENGTH)]
        public string Caliber { get; set; }
    }
}
