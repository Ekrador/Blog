using BLL.Models.Roles;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BLL.Models.Users
{
    public class UserEditApiModel
    {
        [JsonIgnore]
        public string? Id { get; set; }
        public List<string>? Roles { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? MiddleName { get; set; }
        public string? Avatar { get; set; }
        public string? About { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
