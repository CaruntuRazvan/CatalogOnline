namespace CatalogOnline.Models
{
    public class SecretaryHomeViewModel
    {
     
        public List<SecretaryCourseViewModel> Courses { get; set; }
    }

    public class SecretaryCourseViewModel
    {
        public string CourseName { get; set; }
        public string ProfessorName { get; set; }
       
        public List<SecretaryStudentViewModel> Students { get; set; }
        public int CourseId { get; set; }
    }

    public class SecretaryStudentViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Grade { get; set; }
    }
}
