using BLL.Models.Roles;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        public RoleController(RoleManager<Role> roleManager, IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleManager = roleManager;
            _roleService = roleService;
            _logger = logger;
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        [Route("API/Role/Create")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.CreateRole(model);
                if (result.Succeeded)
                {
                    _logger.LogInformation("new role created");
                    return StatusCode(200);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return StatusCode(400);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPatch]
        [Route("API/Role/Edit")]
        public async Task<IActionResult> EditRole([FromBody] EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.EditRole(model);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"edited role {model.Id}");
                    return StatusCode(200);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return StatusCode(404);
        }

        [HttpDelete]
        [Authorize(Roles = "Администратор, Модератор")]
        [Route("API/Role/RemoveRole")]
        public async Task<IActionResult> RemoveRole([FromBody] string id)
        {
            var result = await _roleService.RemoveRole(id);
            if (result.Succeeded)
            {
                _logger.LogWarning($"removed role {id}");
                return StatusCode(200);
            }
            else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        [Route("API/Role/AllRoles")]
        public async Task<List<Role>> AllRoles()
        {
            var roles = await _roleService.GetAllRoles();

            return roles;
        }

        [HttpGet]
        [Route("API/Role/ViewRole")]
        public async Task<RoleViewModel> ViewRole([FromBody] string id)
        {
            var role = await _roleService.ViewRole(id);
            return role;
        }
    }
}
