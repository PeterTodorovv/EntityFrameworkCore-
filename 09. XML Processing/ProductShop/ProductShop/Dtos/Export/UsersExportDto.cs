using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("users")]
    public class UsersExportDto
    {
        [XmlArray("User")]
        public UserEmportDto[] User { get; set; }
    }
}
