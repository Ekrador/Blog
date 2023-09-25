using AutoMapper;
using BLL.Extensions;
using BLL.Models.Users;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    internal class UserService : IUserService
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> EditAccount(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.Convert(model);
            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<User> GetAccount(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<List<User>> GetAccounts()
        {
            var accounts = _userManager.Users.Select(u => u).ToList();           
            return accounts;
        }

        public async Task<IdentityResult> Register(UserRegisterViewModel model)
        {
            var user = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(user, model.PasswordReg);
            if (result.Succeeded)
            {
                var currentUser = await _userManager.FindByIdAsync(user.Id);
                await _userManager.AddToRoleAsync(currentUser, "Пользователь");
                await _signInManager.SignInAsync(user, false);
                return result;
            }
            else
            {
                return result;
            }
        }

        public async Task RemoveAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }
    }
}
