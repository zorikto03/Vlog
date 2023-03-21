using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NetCore3MVC.Models.SupportingModels
{
    public class NewsCreatingModel
    {
        [Required(ErrorMessage = "Не указан заголовок")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Не введено содержание")]
        public string Content { get; set; }
        public IFormFile file { get; set; }
    }
}
