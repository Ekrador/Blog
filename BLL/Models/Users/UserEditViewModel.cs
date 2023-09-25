using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Users
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string LastName { get; set; }

        [EmailAddress]
        [Display(Name = "Email", Prompt = "example@example.com")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        public string UserName => Email;

        [DataType(DataType.Text)]
        [Display(Name = "Отчество", Prompt = "Введите отчество")]
        public string MiddleName { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Аватар", Prompt = "Укажите ссылку на картинку")]
        public string Avatar { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "О себе", Prompt = "Введите данные о себе")]
        public string About { get; set; }
        public DateTime RegistrationDate { get; }
        public List<DAL.Models.Post> Posts { get; }
        public List<Comment> Comments { get; }
    }
}
