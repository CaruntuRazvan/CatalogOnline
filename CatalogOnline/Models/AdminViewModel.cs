using System.Collections.Generic;

namespace CatalogOnline.Models
{
    public class AdminViewModel
    {
        public List<CourseProfessorStudentsViewModel> Courses { get; set; }
        public List<User> Professors { get; set; } // lista de profesori

        public List<User> Students { get; set; } // lista de studenți în model
        public List<User> Secretaries { get; set; } // lista de secretare în model
    }
}
