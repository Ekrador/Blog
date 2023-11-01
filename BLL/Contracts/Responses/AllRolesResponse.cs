using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts.Responses
{
    public class AllRolesResponse
    {
        public int RolesAmount { get; set; }
        public List<RoleViewResponse> Roles { get; set; }
    }
    public class RoleViewResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string> UsersIds { get; set; } 
    }
}
