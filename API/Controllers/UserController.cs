using AutoMapper;
using BLL.Contracts.Responses;
using BLL.Models.Users;
using BLL.Services.IServices;
using Blog;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер пользователей
    /// </summary>
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="model">Данные в формате JSON</param>
        [Route("API/User/Authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(model);

                if (result.Succeeded)
                {
                    _logger.LogInformation("пользователь успешно вошел в систему");
                    return StatusCode(200, "Успешный вход в систему!");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Выход из системы
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("API/User/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            _logger.LogInformation("пользователь вышел из системы");
            return StatusCode(200, "Выход из системы");
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="model">Данные в формате JSON</param>
        [HttpPost]
        [Route("API/User/Register")]
        public async Task<IActionResult> RegisterAccount([FromBody] UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(model);
                if (result.Succeeded)
                {
                    _logger.LogInformation("создан новый аккаунт");
                    return StatusCode(200, "Создан новый аккаунт");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Редактировать пользователя
        /// </summary>
        /// <remarks>Требуется вход в систему под владельцем аккаунта или администратором/модератором</remarks>
        /// <param name="id">Id пользователя</param>
        /// <param name="model">Данные в формате JSON</param>
        [AuthorizationEditUser]
        [Route("API/User/Edit/{id}")]
        [HttpPatch]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] UserEditApiModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = id;
                var result = await _userService.EditAccountFromApi(model);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"информация пользователя изменена: {model.Id}");
                    return StatusCode(200, $"Информация пользователя изменена: {model.Id}");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <remarks>Требуется вход в систему под владельцем аккаунта или администратором/модератором</remarks>
        /// <param name="id">Id пользователя</param>
        [AuthorizationEditUser]
        [HttpDelete]
        [Route("API/User/RemoveAccount/{id}")]
        public async Task<IActionResult> RemoveAccount([FromRoute]string id)
        {
            var result = await _userService.RemoveAccount(id);
            if (result.Succeeded)
            {
                _logger.LogWarning($"аккаунт удален: {id}");
                return StatusCode(200, $"Аккаунт удален: {id}");
            }
            else
            {
                return StatusCode(404, "Пользователь с таким Id не найден");
            }
        }

        /// <summary>
        /// Список пользователей
        /// </summary>
        [HttpGet]
        [Route("API/User/AllAccounts")]
        public async Task<AllUsersResponse> AllUsersResponse()
        {
            var users = await _userService.GetAccountsResponse();
            return users;
        }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        /// <param name="id">Id пользователя</param>
        [HttpGet]
        [Route("API/User/View/{id}")]
        public async Task<UserViewResponse> UserById([FromRoute] string id)
        {
            var user = await _userService.GetAccount(id);
            
            return user;
        }
    }
}
