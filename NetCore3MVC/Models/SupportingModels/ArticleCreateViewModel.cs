using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore3MVC.Models
{
    public class ArticleCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан заголовок")]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(2000, ErrorMessage = "Content имеет ограничение 2000 символов")]
        [Required(ErrorMessage = "Содержание не заполнено")]
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        [Required(ErrorMessage = "Не указаны категории для статьи")]
        public List<int> Categories { get; set; }
    }
}
