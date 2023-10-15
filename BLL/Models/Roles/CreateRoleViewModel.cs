using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Roles
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Введите название роли")]
        [DataType(DataType.Text)]
        [Display(Name = "Name", Prompt = "Введите название роли")]
        public string Name { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Description", Prompt = "Введите описание роли")]
        public string? Description { get; set; }
    }
}
