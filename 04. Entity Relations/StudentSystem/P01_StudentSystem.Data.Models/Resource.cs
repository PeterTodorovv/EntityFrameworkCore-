using P01_StudentSystem.Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        /*
         * •	Resource:
o	ResourceId
o	Name - (up to 50 characters, unicode)
o	Url - (not unicode)
o	ResourceType - (enum – can be Video, Presentation, Document or Other)
o	CourseId

         */

        [Key]
        public int ResourceId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Url { get; set; }

        [Required]
        public ResourseType ResourseType { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}
