using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        /*•	Course:
o	CourseId
o	Name - (up to 80 characters, unicode)
o	Description - (unicode, not required)
o	StartDate
o	EndDate
o	Price
*/
        public Course()
        {
            StudentCourses = new HashSet<StudentCourse>();
            Resources = new HashSet<Resource>();
            HomeworkSubmissions = new HashSet<HomeworkSubmissions>();
        }
        [Key]
        public int CourseId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(80)")]
        [MaxLength(80)]
        public string Name { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
        public ICollection<Resource> Resources { get; set; }
        public ICollection<HomeworkSubmissions> HomeworkSubmissions { get; set; }
    }
}
