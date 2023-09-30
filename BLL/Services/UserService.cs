using AutoMapper;
using BLL.Extensions;
using BLL.Models.Users;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
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

        public async Task<SignInResult> Login(UserLoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user == null)
                return SignInResult.Failed;
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            return result;
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
                var userRole = new Role() { Name = "Пользователь", Description = "Стандартная роль" };
                await _roleManager.CreateAsync(userRole);
                await _userManager.AddToRoleAsync(currentUser, userRole.Name);
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

        public async Task GenerateData()
        {
            var testUser1 = new UserRegisterViewModel { Email = "q@gmail.com", PasswordReg = "12345", PasswordConfirm = "12345", FirstName = "Ivan", LastName = "I" };
            var testUser2 = new UserRegisterViewModel { Email = "qw@gmail.com", PasswordReg = "12345", PasswordConfirm = "12345", FirstName = "Oleg", LastName = "O" };
            var testUser3 = new UserRegisterViewModel { Email = "qwe@gmail.com", PasswordReg = "12345", PasswordConfirm = "12345", FirstName = "Vasya", LastName = "V" };

            var user1 = _mapper.Map<User>(testUser1);
            var user2 = _mapper.Map<User>(testUser2);
            var user3 = _mapper.Map<User>(testUser3);

            var userRole = new Role() { Name = "Пользователь", Description = "Стандартная роль" };
            var moderRole = new Role() { Name = "Модератор", Description = "Права на редактирование статей и комментариев" };
            var adminRole = new Role() { Name = "Администратор", Description = "Максимальные права" };

            await _roleManager.CreateAsync(userRole);
            await _roleManager.CreateAsync(moderRole);
            await _roleManager.CreateAsync(adminRole);

            await _userManager.CreateAsync(user1, testUser1.PasswordReg);
            await _userManager.CreateAsync(user2, testUser2.PasswordReg);
            await _userManager.CreateAsync(user3, testUser3.PasswordReg);

            await _userManager.AddToRoleAsync(user1, "Администратор");
            await _userManager.AddToRoleAsync(user2, "Модератор");
            await _userManager.AddToRoleAsync(user3, "Пользователь");
        }
    }
}
