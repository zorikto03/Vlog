using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCore3MVC.Models;
using NetCore3MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3MVC.Controllers
{
    public class TitleController : Controller
    {
        private readonly VlogContext _context;
        private readonly ILogger<TitleController> _logger;
        
        public TitleController(ILogger<TitleController> logger, VlogContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int pageSize = 10, int page = 1)
        {
            var articles = _context.Articles.ToList().OrderByDescending(x => x.CreateTimestamp);
            var indexViewModel = CreateIndexViewModel(articles, pageSize, page);

            return View(indexViewModel);
        }


        [HttpGet]
        [Authorize]
        public IActionResult CreateArticle()
        {
            var login = User.Identities.FirstOrDefault().Claims
                .FirstOrDefault(x => x.Type == System.Security.Claims.ClaimsIdentity.DefaultNameClaimType).Value;
            var user = _context.Users.FirstOrDefault(x => x.Userlogin == login);
            var categories = _context.ArticleCategories
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                .ToList();


            ViewBag.User = user;
            ViewBag.ArticleCategories = categories;
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateArticle(ArticleCreateViewModel articleCreate)
        {
            if (articleCreate != null)
            {
                var categories = _context.ArticleCategories.Where(x => articleCreate.Categories.Contains(x.Id)).ToList();
                
                if (categories.Count > 0 && !articleCreate.Title.Equals(string.Empty) && articleCreate.Title != null 
                    && !articleCreate.Content.Equals(string.Empty) && articleCreate.Content != null)
                {
                    Article article = new Article()
                    {
                        Id= articleCreate.Id,
                        Title = articleCreate.Title,
                        Content = articleCreate.Content,
                        CreateTimestamp = System.DateTime.Now,
                        AuthorId = articleCreate.AuthorId,
                        Categories = categories
                    };
                
                    _context.Articles.Add(article);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Title");
                }
                //else
                //{
                //    ViewBag.Alert = AlertMaker.ShowAlert(Enums.AlertType.Warning, "Не указан заголовок/контент/категория");
                //}
            }
            return RedirectToAction("CreateArticle");
        }

        [HttpGet]
        public IActionResult Article(int id)
        {
            var article = _context.Articles.Include(x=>x.Categories).FirstOrDefault(x => x.Id == id);
            //var categories = _context.ArticleCategories.Where(x=> article. x.Id)
            
            ViewBag.Article = article;
            ViewBag.Author = _context.Users.FirstOrDefault(x => x.Id == article.AuthorId);
            return View();
        }

        [HttpPost]
        public IActionResult SearchArticles(string search)
        {
            //var res = _context.Articles.Include(x => x.Categories).Where(x => x.Categories != null ? x.Categories.Where(x => x.Name.ContainsCaseInsensitive(search)).Count() > 0 : false); // categories condition
            StringComparison s = StringComparison.OrdinalIgnoreCase;

            var result = _context.Articles
                .Where(x => x.Title.Contains(search) || x.Content.Contains(search))
                .ToList();

            var byCats = SearchArticleByCategories(search);
            byCats.ForEach(x =>
            {
                if (!result.Contains(x))
                {
                    result.Add(x);
                }
            });
            var byAuthor = SearchArticleByAuthor(search);
            byAuthor.ForEach(x =>
            {
                if (!result.Contains(x))
                {
                    result.Add(x);
                }
            });

            var indexViewModel = CreateIndexViewModel(result.OrderByDescending(x => x.CreateTimestamp));

            return View("Index", indexViewModel);
        }

        IndexViewModel<Article> CreateIndexViewModel(IOrderedEnumerable<Article> articles, int pageSize = 10, int page = 1)
        {
            var count = articles.Count();
            var items = articles.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageView = new PageViewModel(count, page, pageSize);
            IndexViewModel<Article> indexViewModel = new IndexViewModel<Article>()
            {
                PageViewModel = pageView,
                Items = items
            };
            return indexViewModel;
        }

        List<Article> SearchArticleByCategories(string category)
        {
            List<Article> articles = new List<Article>();

            var categories = _context.ArticleCategories.Where(x=>x.Name.Contains(category)).ToList();
            foreach(var cat in categories)
            {
                var artcls = _context.Articles.Include(x=>x.Categories).Where(x => x.Categories.Contains(cat)).ToList();
                artcls.ForEach(x => {
                    if (!articles.Contains(x))
                        articles.Add(x);
                });
            }
            return articles;
        }

        List<Article> SearchArticleByAuthor(string search)
        {
            List<Article> articles = new List<Article>();
            
            var author = _context.Users.FirstOrDefault(x => x.Firstname.Contains(search) 
                || x.Secondname.Contains(search) 
                || x.Userlogin.Contains(search));

            if (author != null)
            {
                articles = _context.Articles.Where(x=>x.AuthorId == author.Id).ToList();
            }
            return articles;
        }
    }
}
