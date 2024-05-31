namespace CatalogOnline.Models
{
    public class StudentViewModel
    {
        public User Student { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Grade> Grades { get; set; }

        
    }
}
