﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Artillery.DataProcessor.ImportDto
{
    public class GunCountriesDto
    {
        [Required]
        public int Id { get; set; }
    }
}
