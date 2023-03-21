using System;
using System.Collections.Generic;

#nullable disable

namespace NetCore3MVC.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Userlogin { get; set; }
        public string Userpassword { get; set; }
        public string Gender { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }
        public string AvatarPath { get; set; }
    }
}
