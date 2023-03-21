using Microsoft.AspNetCore.Http;
using System;

namespace NetCore3MVC.Models.SupportingModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public IFormFile file { get; set; }
    }
}
