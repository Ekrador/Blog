using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLL.Models.Comments
{
    public class CreateCommentViewModel
    {
        [Required(ErrorMessage = "Поле Содержание обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Содержание", Prompt = "Содержание")]
        public string Content { get; set; }
        [JsonIgnore]
        public string? AuthorId { get; set; }
        [Required]
        public string PostId { get; set; }
    }
}
