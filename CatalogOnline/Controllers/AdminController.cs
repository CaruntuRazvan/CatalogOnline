using CatalogOnline.ContextModels;
using CatalogOnline.Logic;
using CatalogOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CatalogOnline.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly CatalogContext _context; 

        public AdminController(ILogger<AdminController> logger, CatalogContext context)
        {
            _logger = logger;
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [HttpGet("/UserHome/AdminHome")]
        public IActionResult AdminHome()
        {
            var courses = _context.Courses
                .Include(c => c.Professor)
                .ToList();

            var professors = _context.Users
                .Where(u => u.Role == UserType.Professor) 
                .ToList();

            var students = _context.Users
                .Where(u => u.Role == UserType.Student) 
                .ToList();
            var secretaries = _context.Users
                .Where(u => u.Role == UserType.Secretary) 
                .ToList();

            var adminViewModel = new AdminViewModel
            {
                Courses = courses.Select(course => new CourseProfessorStudentsViewModel
                {
                    CourseId = course.CourseId, // Adăuga CourseId în model
                    CourseName = course.CourseName,
                    ProfessorName = GetProfessorName(course.ProfessorId),
                    Students = GetStudentsNames(course.CourseId)
                }).ToList(),
                Professors = professors,
                Students = students,
                Secretaries = secretaries
            };

            return View("~/Views/UserHome/AdminHome.cshtml", adminViewModel);
        }


        private string GetProfessorName(int professorId)
        {
            var professor = _context.Users.FirstOrDefault(u => u.UserId == professorId);
            
            return professor != null ? $"{professor.FirstName} {professor.LastName}" : "Unknown Professor";
        }

        private List<string> GetStudentsNames(int courseId)
        {
            var students = _context.Enrollments
                            .Where(enrollment => enrollment.CourseId == courseId)
                            .Select(enrollment =>
                                _context.Users.FirstOrDefault(u => u.UserId == enrollment.StudentId) != null ?
                                $"{_context.Users.FirstOrDefault(u => u.UserId == enrollment.StudentId).FirstName} {_context.Users.FirstOrDefault(u => u.UserId == enrollment.StudentId).LastName}" :
                                null)
                            .ToList();
            return students ?? new List<string>();
        }



        [HttpPost("/AdminViews/AddCourse")]
        public IActionResult AddCourse(string courseName, int professorId)
        {
            if (!string.IsNullOrEmpty(courseName) && professorId > 0)
            {
                var course = new Course
                {
                    CourseName = courseName,
                    ProfessorId = professorId
                };
                _context.Courses.Add(course);
                _context.SaveChanges();
                return RedirectToAction("AdminHome");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost("/AdminViews/AddProfessor")]
        public IActionResult AddProfessor(string professorFirstName, string professorLastName)
        {
            if (!string.IsNullOrEmpty(professorFirstName) && !string.IsNullOrEmpty(professorLastName))
            {
                var professor = new User
                {
                    FirstName = professorFirstName,
                    LastName = professorLastName,
                    Role = UserType.Professor
                };

                _context.Users.Add(professor);
                _context.SaveChanges();
                return RedirectToAction("AdminHome");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        [HttpPost("/AdminViews/AddUser")]
        public IActionResult AddUser(string username, string password, string passwordConfirm, string firstName, string lastName, string role)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(passwordConfirm) && !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(role))
            {
                if (password == passwordConfirm)
                {
                    var userRole = Enum.Parse<UserType>(role);

                    var user = new User
                    {
                        Username = username,
                        Password = password,
                        PasswordConfirm = passwordConfirm,
                        FirstName = firstName,
                        LastName = lastName,
                        Role = userRole
                    };

                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("AdminHome");
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost("/AdminViews/AssignStudentToCourse")]
        public IActionResult AssignStudentToCourse(int studentId, int courseId, int academicYear)
        {
            var student = _context.Users.FirstOrDefault(u => u.UserId == studentId);
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);

            if (student != null && course != null)
            {
                
                var enrollment = new Enrollment
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    AcademicYear = academicYear
                };

                _context.Enrollments.Add(enrollment);
                _context.SaveChanges();

                string alertMessage = $"Ai fost adaugat la un curs nou: {course.CourseName}.";
                TempData["AlertMessage"] = alertMessage;
                return RedirectToAction("AdminHome");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost("/AdminViews/DeleteCourse")]
        public IActionResult DeleteCourse(int courseId)
        {
            var courseToDelete = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);
            if (courseToDelete != null)
            {
                _context.Courses.Remove(courseToDelete);
                _context.SaveChanges();
                return RedirectToAction("AdminHome");
            }
            else
            {
                // eroare
                return RedirectToAction("Error");
            }
        }

        [HttpPost("/AdminViews/DeleteUser")]
        public IActionResult DeleteUser(string firstName, string lastName)
        {
            var userToDelete = _context.Users.FirstOrDefault(u => u.FirstName + " " + u.LastName == firstName + " " + lastName);
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                _context.SaveChanges();
                return RedirectToAction("AdminHome");
            }
            else
            {
                
                return RedirectToAction("Error");
            }
        }
        [HttpPost("/AdminViews/DeleteEnrollment")]
        public IActionResult DeleteEnrollment(int studentId, int courseId)
        {
            var enrollmentToDelete = _context.Enrollments.FirstOrDefault(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollmentToDelete != null)
            {
                _context.Enrollments.Remove(enrollmentToDelete);
                _context.SaveChanges();

                return RedirectToAction("AdminHome");
            }
            else
            {
                // Înregistrarea nu a fost găsită, poate doriți să tratați această situație diferit
                return RedirectToAction("Error");
            }
        }


        [HttpPost]
        public IActionResult ResetDatabase()
        {
            _context.ClearDatabase();
            return RedirectToAction("AdminHome"); 
        }
    }
}
