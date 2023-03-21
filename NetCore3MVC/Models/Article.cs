using System;
using System.Collections.Generic;

namespace NetCore3MVC.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public List<ArticleСategory> Categories { get; set; }
    }
}
