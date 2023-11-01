using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLL.Models.News
{
    public class EditNewsViewModel : AddNewsViewModel
    {
        [JsonIgnore]
        public string? Id { get; set; }
    }
}
