namespace CatalogOnline.Models
{
    public class ProfessorViewModel
    {
        public User Professor { get; set; }
        public List<CourseViewModel> Courses { get; set; }
    }
}
