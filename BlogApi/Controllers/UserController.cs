using AutoMapper;
using BLL.Extensions;
using BLL.Models.Users;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("controller")]
    internal class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUserService userService)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userService.GenerateData();
        }
        [Route("User/Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(model);

                if (result.Succeeded)
                {
                    return StatusCode(200);
                }

                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return StatusCode(400);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAccount(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(model);
                if (result.Succeeded)
                {
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
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return StatusCode(400);
            }
            return StatusCode(400);
        }

        [Authorize]
        [Route("Edit")]
        [HttpPut]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditAccount(model);
                if (result.Succeeded)
                {
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(501);
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return StatusCode(400);
            }
        }

        [Authorize(Roles = "Администратор")]
        [HttpDelete]
        [Route("RemoveAccount/{id}")]
        public async Task<IActionResult> RemoveAccount([FromRoute] string id)
        {
            await _userService.RemoveAccount(id);
            return StatusCode(200);
        }

        [HttpGet]
        [Route("AllAccounts")]
        public async Task<List<User>> GetAccounts()
        {
            var users = await _userService.GetAccounts();

            return await Task.FromResult(users);
        }

        [HttpGet]
        [Route("GetAccountById/{id}")]
        public Task<User> GetAccount([FromRoute] string id)
        {
            var users = _userService.GetAccount(id);

            return Task.FromResult(users.Result);
        }
    }
}
