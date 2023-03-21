using System.ComponentModel.DataAnnotations;

namespace NetCore3MVC.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Имя не указано")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия не указана")]
        public string SecondName { get; set; }
    }
}
