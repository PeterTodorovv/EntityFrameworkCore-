using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class CountriesImportDto
    {
        [XmlElement("CountryName")]
        [MaxLength(Constants.COUNTRY_NAME_MAX_LENGTH)]
        [MinLength(Constants.COUNTRY_NAME_MIN_LENGTH)]
        public string CountryName { get; set; }

        [XmlElement("ArmySize")]
        public string ArmySize { get; set; }
    }
}
