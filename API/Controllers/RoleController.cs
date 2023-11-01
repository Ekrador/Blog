using BLL.Contracts.Responses;
using BLL.Models.Roles;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер ролей
    /// </summary>
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

        /// <summary>
        /// Создать новую роль
        /// </summary>
        /// <remarks>Требуется вход в систему под администратором/модератором</remarks>
        /// <param name="model">Данные  в формате JSON</param>
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
                    return StatusCode(200, $"Создана новая роль \"{model.Name}\"");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return BadRequest(model);
        }
        /// <summary>
        /// Редактировать роль
        /// </summary>
        /// <remarks>Требуется вход в систему под администратором/модератором</remarks>
        /// <param name="id">Id роли</param>
        /// <param name="model">Данные в формате JSON</param>
        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPatch]
        [Route("API/Role/Edit/{id}")]
        public async Task<IActionResult> EditRole([FromRoute] string id, [FromBody] EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = id; 
                var result = await _roleService.EditRole(model);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"user {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} edited role {model.Id}");
                    return StatusCode(200, $"Роль {model.Id} изменена пользователем {User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return BadRequest(model);
        }

        /// <summary>
        /// Удалить роль
        /// </summary>
        /// <remarks>Требуется вход в систему под администратором/модератором</remarks>
        /// <param name="id">Id роли</param>
        [HttpDelete]
        [Authorize(Roles = "Администратор, Модератор")]
        [Route("API/Role/RemoveRole/{id}")]
        public async Task<IActionResult> RemoveRole([FromRoute] string id)
        {
            var result = await _roleService.RemoveRole(id);
            if (result.Succeeded)
            {
                _logger.LogWarning($"user {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} removed role {id}");
                return StatusCode(200, $"Роль {id} удалена пользователем {User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Список ролей
        /// </summary>
        [HttpGet]
        [Route("API/Role/AllRoles")]
        public async Task<AllRolesResponse> AllRoles()
        {
            var roles = await _roleService.GetAllRolesResponse();
            return roles;
        }

        /// <summary>
        /// Информация о роли
        /// </summary>
        /// <param name="id">Id роли</param>
        [HttpGet]
        [Route("API/Role/ViewRole/{id}")]
        public async Task<RoleViewResponse> ViewRole([FromRoute] string id)
        {
            var role = await _roleService.ViewRoleResponse(id);
            return role;
        }
    }
}
