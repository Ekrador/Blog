using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLL.Models.Roles
{
    public class EditRoleViewModel : CreateRoleViewModel
    {
        [JsonIgnore]
        public string? Id { get; set; }
    }
}
