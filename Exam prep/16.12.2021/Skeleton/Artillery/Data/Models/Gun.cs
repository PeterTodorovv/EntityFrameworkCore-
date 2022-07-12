using Artillery.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artillery.Data.Models
{
    public class Gun
    {
        public Gun()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Manufacturer))]
        public int ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

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
        [Range(0, 5)]
        public GunType GunType { get; set; }

        [Required]
        [ForeignKey(nameof(Shell))]
        public int ShellId { get; set; }
        public virtual Shell Shell { get; set; }

        public ICollection<CountryGun> CountriesGuns { get; set; }
    }
}
