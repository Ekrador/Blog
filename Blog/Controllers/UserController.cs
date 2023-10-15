using AutoMapper;
using BLL.Extensions;
using BLL.Models.Users;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Blog.Controllers
{
    public class UserController : Controller
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

        [HttpGet]
        [Route("User/Login")]
        public IActionResult Login()
        {
            return View(new UserLoginViewModel());
        }

        [Route("User/Authenticate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authenticate(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(model);

                if (result.Succeeded)
                {
                    _logger.LogInformation("пользователь успешно вошел в систему");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    return View("Login", model);
                }
            }
            ModelState.AddModelError("", "Некорректные данные");
            return View("Login", model);
        }

        [HttpPost]
        [Authorize]
        [Route("User/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            _logger.LogInformation("пользователь вышел из системы");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("User/Register")]
        public IActionResult RegisterAccount()
        {
            return View(new UserRegisterViewModel());
        }

        [HttpPost]
        [Route("User/Register")]
        public async Task<IActionResult> RegisterAccount(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(model);
                if (result.Succeeded)
                {
                    _logger.LogInformation("создан новый аккаунт");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("RegisterAccount", model);
        }

        [AuthorizationEditUser]
        [Route("User/Edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await _userService.EditAccount(id);
            if (model == null)
                return StatusCode(404);
            return View(model);
        }


        [AuthorizationEditUser]
        [Route("User/Edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditAccount(model);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"информация пользователя изменена: {model.Id}");
                    return RedirectToAction("UserPage", "User", new { Id = model.Id });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("Edit", model);
        }

        [AuthorizationEditUser]
        [HttpPost]
        [Route("User/RemoveAccount/{id}")]
        public async Task<IActionResult> RemoveAccount(string id)
        {
            await _userService.RemoveAccount(id);
            _logger.LogWarning($"аккаунт удален: {id}");
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("User/AllAccounts")]
        public async Task<IActionResult> AllUsers()
        {
            var users = await _userService.GetAccounts();
            var model = new AllUsersViewModel { Users = users };
            return View(model);
        }

        [HttpGet]
        [Route("User/View")]
        public async Task<IActionResult> UserPage(string id)
        {
            var user = await _userService.UserPage(id);
            return View(user);
        }
    }
}
