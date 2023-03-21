using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCore3MVC.Models;
using NetCore3MVC.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VlogContext _context;
        
        public HomeController(ILogger<HomeController> logger, VlogContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<User> users = _context.Users.ToList();
                ViewData["users"] = users;
            }

            List<New> news = GetLastestNews();
            ViewData["news"] = news;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        List<New> GetLastestNews()
        {
            var news = _context.News.Include(x=>x.FilesPaths).OrderByDescending(x => x.Id).Take(5).ToList();
            return news;
        }
    }
}
