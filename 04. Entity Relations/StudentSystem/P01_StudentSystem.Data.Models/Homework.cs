using P01_StudentSystem.Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class HomeworkSubmissions
    {
        /*o	HomeworkId
o	Content - (string, linking to a file, not unicode)
o	ContentType - (enum – can be Application, Pdf or Zip)
o	SubmissionTime
o	StudentId
o	CourseId

         */

        [Key]
        public int HomeworkId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public ContentType ContentType { get; set; }

        [Required]
        public DateTime SubmissionTime { get; set; }

        [Required]
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        [Required]
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
