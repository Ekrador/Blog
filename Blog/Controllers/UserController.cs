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

        [HttpGet]
        [Route("User/Login")]
        public IActionResult Login(string returnUrl = null)
        {

            return View(new UserLoginViewModel { ReturnUrl = returnUrl });
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
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
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

        [Authorize(Roles = "Администратор")]
        [HttpPost]
        [Route("User/RemoveAccount")]
        public async Task<IActionResult> RemoveAccount(string id)
        {
            await _userService.RemoveAccount(id);
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
