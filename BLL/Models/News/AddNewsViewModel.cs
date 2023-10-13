using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.News
{
    public class AddNewsViewModel
    {

        [Required(ErrorMessage = "Введите заголовок новости!")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок", Prompt = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Нельзя создать пустую новость!")]
        [DataType(DataType.Text)]
        [Display(Name = "Содержание", Prompt = "Содержание")]
        public string Content { get; set; }
    }
}
