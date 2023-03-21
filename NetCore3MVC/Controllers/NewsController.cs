using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCore3MVC.Models;
using NetCore3MVC.Models.SupportingModels;
using NetCore3MVC.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3MVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly VlogContext _context;
        private readonly ILogger<NewsController> _logger;
        private readonly IWebHostEnvironment _appWebEnvironment;

        public NewsController(VlogContext context, ILogger<NewsController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _appWebEnvironment = webHostEnvironment;
        }

        // GET: NewsController
        public async Task<ActionResult> IndexAsync(int pageSize = 10, int page = 1)
        {
            var news = _context.News.OrderByDescending(x => x.CreateTimestamp);
            var count = await news.CountAsync();
            var items = await news.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageView = new PageViewModel(count, page, pageSize);
            IndexViewModel<New> indexViewModel = new IndexViewModel<New>()
            {
                PageViewModel = pageView,
                Items = items
            };
            return View(indexViewModel);
        }

        [HttpGet]
        public IActionResult NewsPage(int id)
        {
            using (var context = new VlogContext())
            {
                var news = context.News.Include(x=>x.FilesPaths).FirstOrDefault(x => x.Id == id);
                ViewBag.New = news;
            }
            return View();
        }

        // GET: NewsController/Create
        [Authorize(Roles = "Admin")]
        public IActionResult CreateNews()
        {
            return View();
        }

        // POST: NewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateNews(NewsCreatingModel model)
        {
            try
            {
                if(model != null)
                {
                    var type = _context.FileTypes.FirstOrDefault(x => x.Name == "image");
                    if (type != null) 
                    { 
                        var news = new New() { Title = model.Title, Content = model.Content };
                        var path = "/Resources/News/" + model.file.FileName;
                        using (var fileStream = new FileStream(_appWebEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await model.file.CopyToAsync(fileStream);
                        }
                        MediaFile file = new MediaFile() { Path = path, Type = type };
                        _context.MediaFiles.Add(file);

                        news.CreateTimestamp = System.DateTime.Now;
                        news.FilesPaths = new List<MediaFile>() { file };
                        _context.News.Add(news);

                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction("Index", "News");
            }
            catch
            {
                return View();
            }
        }

        // GET: NewsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: NewsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}
