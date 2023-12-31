﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? MiddleName { get; set; }

        public DateTime BirthDate { get; set; }
        public string Avatar { get; set; }

        public string About { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Post> Posts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public List<Role> Roles { get; set; } 
        

        public string GetFullName()
        {
            return LastName + " " + FirstName + " " + MiddleName ;
        }

        public User()
        {
            RegistrationDate = DateTime.UtcNow;
            Posts = new List<Post>();
            Comments = new List<Comment>();
            Avatar = "../images/avatar.png";
            About = "Информация обо мне.";
        }
    }
}
