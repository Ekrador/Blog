using AutoMapper;
using BLL.Models.News;
using BLL.Models.Roles;
using BLL.Models.Tags;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class RoleService : IRoleService
    {
        private IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleService(RoleManager<Role> roleManager, IMapper mapper, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateRole(CreateRoleViewModel model)
        {
            var role = _mapper.Map<Role>(model);
            var result = await _roleManager.CreateAsync(role);
            return result;
        }

        public async Task<EditRoleViewModel> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var model = new EditRoleViewModel { Id = id, Name = role.Name, Description = role.Description};
            return model;
        }

        public async Task<IdentityResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            role.Name = model.Name;
            role.Description = model.Description;
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<RoleViewModel> ViewRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            var model = new RoleViewModel { Id = id, Name = role.Name, Description = role.Description, Users = users.ToList() };
            return model;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            var roles = _roleManager.Roles.AsParallel().ToList();
            return roles;
        }

        public async Task<IdentityResult> RemoveRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return await _roleManager.DeleteAsync(role);
        }
    }
}
