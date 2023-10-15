using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Tags
{
    public class CreateTagViewModel
    {
        [Required(ErrorMessage = "Укажите название тега")]
        [Display(Name = "Name", Prompt = "Введите название тега")]
        public string Name { get; set; }
    }
}
