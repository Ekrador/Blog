using BLL.Contracts.Responses;
using BLL.Models.Roles;
using BLL.Models.Users;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.IServices
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRole(CreateRoleViewModel model);
        Task<EditRoleViewModel> EditRole(string id);
        Task<IdentityResult> EditRole(EditRoleViewModel model);
        Task<IdentityResult> RemoveRole(string id);
        Task<List<Role>> GetAllRoles();
        Task<AllRolesResponse> GetAllRolesResponse();
        Task<RoleViewModel> ViewRole(string id);
        Task<RoleViewResponse> ViewRoleResponse(string id);
    }
}
