using BLL.Contracts.Responses;
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
        Task Logout();
        Task<UserEditViewModel> EditAccount(string id);
        Task<IdentityResult> EditAccount(UserEditViewModel model);
        Task<IdentityResult> EditAccountFromApi(UserEditApiModel model);
        Task<IdentityResult> RemoveAccount(string id);
        Task<List<User>> GetAccounts();
        Task<AllUsersResponse> GetAccountsResponse();
        Task<UserViewResponse> GetAccount(string id);
        Task<UserViewModel> UserPage(string id);
        Task AddPostClaim(User user, string postIdToClaim);
        Task AddCommentClaim(User user, string commentIdToClaim);
        Task GenerateData();
    }
}
