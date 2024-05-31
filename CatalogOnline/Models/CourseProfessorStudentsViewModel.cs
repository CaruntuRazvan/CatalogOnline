namespace CatalogOnline.Models
{
    public class CourseProfessorStudentsViewModel
    {
        public int CourseId { get; set; } // Adăugăm proprietatea CourseId
        public string CourseName { get; set; }
        public string ProfessorName { get; set; }
        public List<string> Students { get; set; }
    }
}