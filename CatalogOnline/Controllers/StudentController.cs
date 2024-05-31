using CatalogOnline.ContextModels;
using CatalogOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CatalogOnline.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly CatalogContext _context; // Adăugăm o referință către contextul bazei de date

        public StudentController(ILogger<StudentController> logger, CatalogContext context)
        {
            _logger = logger;
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("/UserHome/StudentHome")]
        public IActionResult StudentHome(int userId)
        {
            //var student = _context.Users.FirstOrDefault(u => u.UserId == userId);
            var student = _context.Users.FirstOrDefault(u => u.UserId == userId);
            var enrollments = _context.Enrollments
                .Where(e => e.StudentId == userId)
                .Include(e => e.Course)
                .ToList();

            var grades = _context.Grades
                .Where(g => g.StudentId == userId)
                .ToList();

            var studentData = new StudentViewModel
            {
                Student = student,
                Enrollments = enrollments,
                Grades = grades
            };
            var academicYears = _context.Enrollments.Select(e => e.AcademicYear).Distinct().ToList();
            ViewBag.AcademicYears = academicYears;

            if (student != null)
            {
                return View("~/Views/UserHome/StudentHome.cshtml", studentData);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

    }
}
