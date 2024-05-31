using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CatalogOnline.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; } // Cheie primară

        [Required]
        public string CourseName { get; set; } // Numele cursului

        public int ProfessorId { get; set; } // Cheie externă către User (profesorul asociat cursului)

        public User Professor { get; set; } // Proprietate de navigare către profesorul asociat acestui curs
    }
}
