
using CatalogOnline.Models;
using CatalogOnline.ContextModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CatalogOnline.Logic;

namespace CatalogOnline.Controllers;


public class AuthenticationController : Controller
{
    private readonly CatalogContext _context;

    public AuthenticationController(CatalogContext context)
    {
        this._context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Introduction()
    {
        return View();
    }
    /*
    [HttpPost]
    public IActionResult Register(User model)
    {
        
            try
            {
                if (string.IsNullOrWhiteSpace(model.Username))
                    ModelState.AddModelError(string.Empty, "The username is empty!");
                else if (string.IsNullOrWhiteSpace(model.Password))
                    ModelState.AddModelError(string.Empty, "The password is empty!");
                else if (string.IsNullOrWhiteSpace(model.PasswordConfirm))
                    ModelState.AddModelError(string.Empty, "The password confirmation is empty!");
                else if (model.Password != model.PasswordConfirm)
                    ModelState.AddModelError(string.Empty, "The password and password confirmation don't match!");
                else if (_context.Users.Where(user => user.Username!.ToLower() == model.Username.ToLower()).Count() > 0)
                    ModelState.AddModelError(string.Empty, "The username is already taken!");
                else
                {
                    try
                    {
                        _context.Users.Add(model);
                        _context.SaveChanges();
                    // Verificăm direct rolul și executăm logica specifică
                    if (model.Role == UserType.Professor)
                    {
                        // Logica pentru un utilizator cu rol de profesor
                        return RedirectToAction("ProfessorHome", "UserHome");
                    }
                    else if (model.Role == UserType.Student)
                    {
                        // Logica pentru un utilizator cu rol de student
                        return RedirectToAction("StudentHome", "UserHome");
                    }
                    else if (model.Role == UserType.Secretary)
                    {
                        // Logica pentru un utilizator cu rol de student
                        return RedirectToAction("SecretaryHome", "UserHome");
                    }
                    else if (model.Role == UserType.Admin)
                    {
                        // Logica pentru un utilizator cu rol de student
                        return RedirectToAction("AdminHome", "UserHome");
                    }
                    //return RedirectToAction("Index", "Home");
                }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Error creating account: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        
        return View(model);
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    */
    [HttpPost]
    public async Task<IActionResult> Login(User model)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                ModelState.AddModelError(string.Empty, "The username is empty!");
                return View(model); 
            }
            else if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(string.Empty, "The password is empty!");
                return View(model); 
            }
            else
            {
                var user = await _context.Users.FirstOrDefaultAsync(user => user.Username.ToLower() == model.Username.ToLower());

                if (user != null && user.Password == model.Password)
                {
                    List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim("Role", user.Role.ToString())
                };
                    var claimIdentity = new ClaimsIdentity(claims, "AuthenticationCookie");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                    // Verificăm direct rolul și executăm logica specifică
                    if (user.Role == UserType.Professor)
                    {
                        return RedirectToAction("ProfessorHome", "UserHome", new { userId = user.UserId });
                    }
                    else if (user.Role == UserType.Student)
                    {
                        return RedirectToAction("StudentHome", "UserHome", new { userId = user.UserId });
                    }
                    else if (user.Role == UserType.Secretary)
                    {
                        return RedirectToAction("SecretaryHome", "UserHome");
                    }
                    else if (user.Role == UserType.Admin)
                    {
                        return RedirectToAction("AdminHome", "UserHome");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password!");
                    return View(model); 
                }
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Error logging in: " + ex.Message);
            return View(model); // Returnează pagina de login cu mesajul de eroare
        }

        return RedirectToAction("Login");
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

   

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated == false)
            return RedirectToAction("Index", "Home");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }



}

