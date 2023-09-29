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

namespace Blog.Controllers
{
    public class UserController : Controller
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
                    //return StatusCode(200);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Некорректные данные");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        [Route("User/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
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
        [Route("User/Edit")]
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
        [Route("User/RemoveAccount/{id}")]
        public async Task<IActionResult> RemoveAccount([FromRoute] string id)
        {
            await _userService.RemoveAccount(id);
            return StatusCode(200);
        }
        [Authorize(Roles = "Администратор")]
        [HttpGet]
        [Route("User/AllAccounts")]
        public async Task<IActionResult> GetAccounts()
        {
            var users = await _userService.GetAccounts();
            return View(users);
        }
        
        [HttpGet]
        [Route("User/GetAccountById/{id}")]
        public async Task<User> GetAccount([FromRoute] string id)
        {
            var user = await _userService.GetAccount(id);
            return user;
        }
    }
}
