using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Post
{
    public class EditPostViewModel
    {
        public string Id { get; set; }
        public List<Tag>? Tags { get; set; } = new List<Tag>();

        [Required(ErrorMessage = "Введите заголовок статьи!")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок", Prompt = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Нельзя создать пустую статью!")]
        [DataType(DataType.Text)]
        [Display(Name = "Содержание", Prompt = "Содержание")]
        public string Content { get; set; }
    }
}
