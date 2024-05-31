using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatalogOnline.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; } // Cheie primară

        public int StudentId { get; set; } // Cheie externă către studentul înscrierii

        public int AcademicYear { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; } // Cheie externă către cursul înscrierii

        public User Student { get; set; } // Proprietate de navigare către studentul asociat acestei înscrieri
        public Course Course { get; set; } // Proprietate de navigare către cursul asociat acestei înscrieri
    }
}
