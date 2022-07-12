using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Artillery.Data.Models
{
    public class Country
    {
        public Country()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Constants.COUNTRY_NAME_MAX_LENGTH)]
        public string CountryName { get; set; }

        [Required]
        [Range(Constants.ARMY_SIZE_MIN_VALUE, Constants.ARMY_SIZE_MAX_VALUE)]
        public int ArmySize { get; set; }

        public ICollection<CountryGun> CountriesGuns { get; set; }
    }
}
