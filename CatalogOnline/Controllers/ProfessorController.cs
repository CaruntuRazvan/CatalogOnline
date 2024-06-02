using CatalogOnline.ContextModels;
using CatalogOnline.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace CatalogOnline.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly ILogger<ProfessorController> _logger;
        private readonly CatalogContext _context; 

        public ProfessorController(ILogger<ProfessorController> logger, CatalogContext context)
        {
            _logger = logger;
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("/UserHome/ProfessorHome")]
        public IActionResult ProfessorHome(int userId)
        {
            var professor = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (professor != null)
            {
                var courses = _context.Courses
                    .Where(c => c.ProfessorId == userId)
                    .Select(c => new CourseViewModel
                    {
                        Course = c,
                        Students = _context.Enrollments
                            .Where(e => e.CourseId == c.CourseId)
                            .Select(e => new StudentViewModel
                            {
                                Student = e.Student,
                                Enrollments = _context.Enrollments.Where(en => en.StudentId == e.StudentId).ToList(),
                                Grades = _context.Grades.Where(g => g.StudentId == e.StudentId && g.CourseId == c.CourseId).ToList()
                            })
                            .ToList()
                    })
                    .ToList();

                var professorData = new ProfessorViewModel
                {
                    Professor = professor,
                    Courses = courses
                };

                return View("~/Views/UserHome/ProfessorHome.cshtml", professorData);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        [HttpPost]
        public IActionResult AddGrade(AddGradeViewModel model)
        {
            // Verificăm dacă studentul și cursul există
            var student = _context.Users.FirstOrDefault(u => u.UserId == model.StudentId);
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == model.CourseId);
            var professor = _context.Users.FirstOrDefault(u => u.UserId == model.ProfessorId);
            Console.WriteLine("AICI SA TE UITI ->ProfessorId: " + model.StudentId);

            Console.WriteLine("AICI SA TE UITI ->ProfessorId: " + model.CourseId);

            Console.WriteLine("AICI SA TE UITI ->ProfessorId: " + model.ProfessorId);


            if (student != null && course != null)
            {
                // Creăm noul obiect Grade
                var grade = new Grade
                {
                    StudentId = model.StudentId,
                    CourseId = model.CourseId,
                    GradeMark = model.GradeMark
                };

                _context.Grades.Add(grade);
                _context.SaveChanges();

                // Generăm mesajul de alertă
                string alertMessage = $"A fost adaugata o nota pentru cursul {course.CourseName}. Nota ta este {model.GradeMark}.";
                TempData["AlertMessage"] = alertMessage;
                
                
                return RedirectToAction("ProfessorHome", new { userId = model.ProfessorId });

            }

            return RedirectToAction("Error");

        }

        [HttpPost]
        public IActionResult DeleteGrade(DeleteGradeViewModel model)
        {
            
            var grade = _context.Grades.FirstOrDefault(g => g.GradeId == model.GradeId);
            if (grade != null)
            {
                _context.Grades.Remove(grade);
                _context.SaveChanges();

                return RedirectToAction("ProfessorHome", new { userId = model.ProfessorId });
            }

            return RedirectToAction("Error");
        }


    }
}
