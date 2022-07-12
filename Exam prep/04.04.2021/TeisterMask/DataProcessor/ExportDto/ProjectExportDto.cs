using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using TeisterMask.DataProcessor.ImportDto;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ProjectExportDto
    {
        [XmlAttribute("TasksCount")]
        public string TasksCount { get; set; }

        [XmlElement("ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public TaskExportDto[] Tasks { get; set; } 
    }
}
