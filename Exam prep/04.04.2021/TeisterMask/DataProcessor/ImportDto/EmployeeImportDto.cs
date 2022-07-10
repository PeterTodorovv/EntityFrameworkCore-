using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class EmployeeImportDto
    {
        [Required]
        [MaxLength(Constants.UsernameMaxLength)]
        [MinLength(Constants.UsernameMinLength)]
        [RegularExpression(Constants.UsernameRegex)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(Constants.PhoneRegex)]
        public string Phone { get; set; }

        public int[] Tasks { get; set; }
    }
}
