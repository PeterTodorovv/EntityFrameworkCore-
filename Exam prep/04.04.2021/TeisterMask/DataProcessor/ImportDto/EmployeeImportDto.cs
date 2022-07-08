using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class EmployeeImportDto
    {
        [Required]
        [MaxLength(Constants.USERNAME_MAX_LENGTH)]
        [MinLength(Constants.USERNAME_MIN_LENGTH)]
        [RegularExpression(Constants.USERNAME_REGEX)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(Constants.PHONE_REGEX)]
        public string Phone { get; set; }
        public int[] Tasks { get; set; }
    }
}
