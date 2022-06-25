using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Users")]
    public class UsersWithSoldProductsOutputDto
    {
        [XmlElement("count")]
        public string Count { get; set; }

        [XmlArray("users")]
        public UsersExportDto[] Users { get; set; }
    }
}
