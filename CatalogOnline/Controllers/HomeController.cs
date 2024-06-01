using CatalogOnline.ContextModels;
using CatalogOnline.Logic;
using CatalogOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace CatalogOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CatalogContext _context; // Adăugăm o referință către contextul bazei de date

        public HomeController(ILogger<HomeController> logger, CatalogContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);

        }

        public IActionResult Introduction()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }


    }
}

