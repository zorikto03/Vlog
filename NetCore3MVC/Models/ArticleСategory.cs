using System.Collections.Generic;

namespace NetCore3MVC.Models
{
    public class ArticleСategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Article> Articles { get; set; }
    }
}
