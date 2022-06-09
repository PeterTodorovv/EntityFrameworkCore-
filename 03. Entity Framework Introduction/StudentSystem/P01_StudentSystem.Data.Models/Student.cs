using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        /*o	StudentId
o	Name - (up to 100 characters, unicode)
o	PhoneNumber - (exactly 10 characters, not unicode, not required)
o	RegisteredOn
o	Birthday - (not required)

         */
        public Student()
        {
            HomeworkSubmissions = new HashSet<HomeworkSubmissions>();
            StudentCourses = new HashSet<StudentCourse>();
        }
        [Key]
        public int StudentId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string Name { get ; set; }

        [MaxLength(10)]
        [MinLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }


        public ICollection<HomeworkSubmissions> HomeworkSubmissions { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
