using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Artillery.DataProcessor.ImportDto
{
    public class GunsImportDto
    {
        [Required]
        public int ManufacturerId { get; set; }

        [Required]
        [Range(Constants.GUN_MIN_WEIGHT, Constants.GUN_MAX_WEiGHT)]
        public int GunWeight { get; set; }

        [Required]
        [Range(Constants.BARREL_MIN_LENGTH, Constants.BARREL_MAX_LENGTH)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Required]
        [Range(Constants.MIN_RANGE, Constants.MAX_RANGE)]
        public int Range { get; set; }

        [Required]
        public string GunType { get; set; }

        [Required]
        public int ShellId { get; set; }

        public GunCountriesDto[] Countries { get; set; }
    }
}
