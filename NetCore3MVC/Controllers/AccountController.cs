using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCore3MVC.Models;
using NetCore3MVC.Models.SupportingModels;
using NetCore3MVC.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCore3MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly VlogContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly IWebHostEnvironment _appWebEnvironment;
        public AccountController(ILogger<AccountController> logger, VlogContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _appWebEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(x => x.Userlogin == model.Login);
                Role role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == "User");
                if (user == null)
                {
                    _context.Users.Add(new Models.User()
                    {
                        Userlogin = model.Login,
                        Userpassword = model.Password,
                        Firstname = model.FirstName,
                        Secondname = model.SecondName,
                        Role = role
                    });

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином существует");
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Userlogin == model.Login && x.Userpassword == model.Password);
                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Alert = AlertMaker.ShowAlert(Enums.AlertType.Warning, "Некорректный логин и/или пароль");
               // ModelState.AddModelError(string.Empty, "Некорректный логин и/или пароль");
            }
            return View(model);
        }

        //this View for changing personal data like names, country, city and etc.
        public IActionResult Person()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _context.Users.FirstOrDefault(x => x.Userlogin == User.Identity.Name);
                if (user != null)
                {
                    ViewBag.User = user;
                    return View();
                }
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Save(UserViewModel user)
        {
            var currentUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            if (currentUser != null)
            {
                if (user.file != null)
                {
                    var path = "/Resources/Avatar/" + user.file.FileName;
                    using (var stream = new FileStream(_appWebEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await user.file.CopyToAsync(stream);
                    }
                    currentUser.AvatarPath = path;
                }

                currentUser.Firstname = user.Firstname;
                currentUser.Secondname = user.Secondname;
                currentUser.Birthday = user.Birthday;
                currentUser.Gender = user.Gender;
                currentUser.City = user.City;
                currentUser.Country = user.Country;
            }

            var isValid = TryValidateModel(currentUser);
            if (!isValid)
            {
                ViewBag.Alert = AlertMaker.ShowAlert(Enums.AlertType.Warning, "Error! Something wrong");
                return RedirectToAction("Person");
            }

            _context.Update(currentUser);
            _context.SaveChanges();
            return RedirectToAction("Person");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Userlogin),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
                new Claim("FirstName", user.Firstname),
                new Claim("SecondName", user.Secondname)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
