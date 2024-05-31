using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatalogOnline.Models
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; } // Cheie primară

        [Required]
        public int GradeMark { get; set; } // Nota

        [ForeignKey("Course")]
        public int CourseId { get; set; } // Cheie externă către cursul asociat acestei note

       
        public int StudentId { get; set; } // Cheie externă către studentul asociat acestei note

        public Course Course { get; set; } // Proprietate de navigare către cursul asociat acestei note
        public User Student { get; set; } // Proprietate de navigare către studentul asociat acestei note
    }
}
