using BLL.Models.Users;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.IServices
{
    public interface IUserService
    {
        Task<IdentityResult> Register(UserRegisterViewModel model);
        Task<SignInResult> Login(UserLoginViewModel model);
        Task<IdentityResult> EditAccount(UserEditViewModel model);
        Task RemoveAccount(string id);
        Task<List<User>> GetAccounts();
        Task<User> GetAccount(string id);
        Task GenerateData();
    }
}
