using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class UsersImportDto
    {
        [Required]
        [RegularExpression(Constants.FullName_regex)]
        public string FullName { get; set; }

        [Required]
        [MinLength(Constants.Username_min_length)]
        [MaxLength(Constants.Username_max_length)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(Constants.Age_min_value, Constants.Age_max_value)]
        public int Age { get; set; }

        public ICollection<CardImportDto> Cards { get; set; }

        public UsersImportDto() 
        {
            Cards = new List<CardImportDto>();
        }
    }
}
