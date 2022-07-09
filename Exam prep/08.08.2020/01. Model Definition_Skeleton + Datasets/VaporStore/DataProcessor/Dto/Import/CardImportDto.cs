using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class CardImportDto
    {
        [Required]
        [RegularExpression(Constants.Card_number_regex)]
        public string Number { get; set; }

        [Required]
        [MaxLength(Constants.Cvc_length)]
        [MinLength(Constants.Cvc_length)]
        public string CVC { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
