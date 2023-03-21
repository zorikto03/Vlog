using System.ComponentModel.DataAnnotations;

namespace NetCore3MVC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
