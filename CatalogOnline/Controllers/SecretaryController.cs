using CatalogOnline.ContextModels;
using CatalogOnline.Models;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using System.Linq;
using iText.Layout;

namespace CatalogOnline.Controllers
{
    public class SecretaryController : Controller
    {

        private readonly ILogger<SecretaryController> _logger;
        private readonly CatalogContext _context; // Adăugăm o referință către contextul bazei de date

        public SecretaryController(ILogger<SecretaryController> logger, CatalogContext context)
        {
            _logger = logger;
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [HttpGet("/UserHome/SecretaryHome")]
        public IActionResult SecretaryHome()
        {
            var courses = _context.Courses
                .Include(c => c.Professor)
                .ToList();

            var secretaryHomeViewModel = new SecretaryHomeViewModel
            {
                Courses = courses.Select(course =>
                {
                    var studentsWithGrades = GetStudentsWithGrades(course.CourseId);
                    return new SecretaryCourseViewModel
                    {
                        CourseName = course.CourseName,
                        ProfessorName = GetProfessorName(course.ProfessorId),
                        Students = studentsWithGrades
                    };
                }).ToList()
            };

            return View("~/Views/UserHome/SecretaryHome.cshtml", secretaryHomeViewModel);
        }
        private List<SecretaryStudentViewModel> GetStudentsWithGrades(int courseId)
        {
            
            var studentsWithGrades = _context.Enrollments
                .Where(enrollment => enrollment.CourseId == courseId)
                .SelectMany(enrollment =>
                    _context.Grades.Where(grade =>
                        grade.CourseId == courseId && grade.StudentId == enrollment.StudentId)
                    .DefaultIfEmpty(),
                    (enrollment, grade) => new SecretaryStudentViewModel
                    {
                        FirstName = enrollment.Student != null ? enrollment.Student.FirstName : null,
                        LastName = enrollment.Student != null ? enrollment.Student.LastName : null,
                        Grade = grade != null ? (int?)grade.GradeMark : null
                    })
                .ToList();

            return studentsWithGrades;
        }
        private string GetProfessorName(int professorId)
        {
            var professor = _context.Users.FirstOrDefault(u => u.UserId == professorId);
            
            return professor != null ? $"{professor.FirstName} {professor.LastName}" : "Unknown Professor";
        }

        

    }
}
