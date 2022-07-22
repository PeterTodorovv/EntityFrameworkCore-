using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.DataProcessor.ImportDto
{
    public class TicketImportDto
    {

        [Required]
        [Range(Constants.MinPrice, Constants.MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [Range(Constants.MinRowNumber, Constants.MaxRowNumber)]
        public sbyte RowNumber { get; set; }

        [Required]
        public int PlayId { get; set; }

    }
}
