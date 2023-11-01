using BLL.Models.Posts;
using BLL.Models.Roles;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class RoleFromModelExt
    {
        public static Role Convert(this Role role, EditRoleViewModel roleeditvm)
        {
            role.Name = roleeditvm.Name ?? role.Name;
            role.Description = roleeditvm.Description ?? role.Description;
            return role;
        }
    }
}
