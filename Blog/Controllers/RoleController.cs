using BLL.Models.Roles;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IRoleService _roleService;
        public RoleController(RoleManager<Role> roleManager, IRoleService roleService)
        {
            _roleManager = roleManager;
            _roleService = roleService;
        }

        [HttpGet]
        [Route("Role/Create")]
        public async Task<IActionResult> CreateRole()
        {
            return View(new CreateRoleViewModel());
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        [Route("Role/Create")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.CreateRole(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles", "Role");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("CreateRole", model);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        [Route("Role/Edit")]
        public async Task<IActionResult> EditRole(string id)
        {
            var model = await _roleService.EditRole(id);
            if (model == null)
                return StatusCode(404);
            return View(model);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        [Route("Role/Edit")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.EditRole(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("EditRole", model);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Модератор")]
        [Route("Role/RemoveRole")]
        public async Task<IActionResult> RemoveRole(string id)
        {
            await _roleService.RemoveRole(id);
            return RedirectToAction("AllRoles", "Role");
        }

        [HttpGet]
        [Route("Role/AllRoles")]
        public async Task<IActionResult> AllRoles()
        {
            var roles = await _roleService.GetAllRoles();

            return View(new AllRolesViewModel { Roles = roles});
        }

        [HttpGet]
        [Route("Role/ViewRole")]
        public async Task<IActionResult> ViewRole(string id)
        {
            var role = await _roleService.ViewRole(id);
            return View(role);
        }
    }
}
