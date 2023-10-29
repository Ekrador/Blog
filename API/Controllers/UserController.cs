using AutoMapper;
using BLL.Models.Users;
using BLL.Services.IServices;
using Blog;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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

        [Route("API/User/Authenticate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(model);

                if (result.Succeeded)
                {
                    _logger.LogInformation("пользователь успешно вошел в систему");
                    return StatusCode(200);
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            ModelState.AddModelError("", "Некорректные данные");
            return StatusCode(400);
        }

        [HttpPost]
        [Authorize]
        [Route("API/User/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            _logger.LogInformation("пользователь вышел из системы");
            return StatusCode(200);
        }

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


        [AuthorizationEditUser]
        [Route("API/User/Edit")]
        [HttpPatch]
        public async Task<IActionResult> Edit([FromBody] UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditAccount(model);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"информация пользователя изменена: {model.Id}");
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

        [Authorize]
        [HttpDelete]
        [Route("API/User/RemoveAccount/{id}")]
        public async Task<IActionResult> RemoveAccount([FromRoute]string id)
        {
            var result = await _userService.RemoveAccount(id);
            if (result.Succeeded)
            {
                _logger.LogWarning($"аккаунт удален: {id}");
                return StatusCode(200);
            }
            else
            {
                return StatusCode(404);
            }
        }
        [HttpGet]
        [Route("API/User/AllAccounts")]
        public async Task<List<User>> AllUsers()
        {
            var users = await _userService.GetAccounts();
            return users;
        }

        [HttpGet]
        [Route("API/User/View/{id}")]
        public async Task<User> UserById([FromRoute] string id)
        {
            var user = await _userService.GetAccount(id);
            return user;
        }
    }
}
