using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class TaskImportDto
    {
        [Required]
        [XmlElement("Name")]
        [MaxLength(Constants.NameMaxLength)]
        [MinLength(Constants.NameMinLength)]
        public string Name { get; set; }
        
        [Required]
        [XmlElement("OpenDate")]
        public string OpenDate { get; set; }
        
        [Required]
        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [Required]
        [XmlElement("ExecutionType")]
        public string ExecutionType { get; set; }
        
        [Required]
        [XmlElement("LabelType")]
        public string LabelType { get; set; }
    }
}
