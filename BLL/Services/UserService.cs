using AutoMapper;
using BLL.Contracts.Responses;
using BLL.Extensions;
using BLL.Models.Roles;
using BLL.Models.Users;
using BLL.Services.IServices;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IRepository<Post> _postRep;
        private readonly IRepository<Comment> _commentRep;
        private IHttpContextAccessor _httpContextAccessor;
        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager,
            IMapper mapper, IRepository<Post> postrep, IRepository<Comment> commentrep, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _postRep = postrep;
            _commentRep = commentrep;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<SignInResult> Login(UserLoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return SignInResult.Failed;

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            var userPosts = _postRep.GetAll().Result.Where(p => p.Author?.Id == user.Id).Select(p => p.Id).ToList();
            var userComments = _commentRep.GetAll().Result.Where(c => c.Author?.Id == user.Id).Select(c => c.Id).ToList();
            await AddPostsClaim(user, userPosts);
            await AddCommentsClaim(user, userComments);

            return result;
        }

        public async Task<UserEditViewModel> EditAccount(string id)
        {
            UserEditViewModel userModel = null;
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = _roleManager.Roles.ToList();
                userModel = _mapper.Map<UserEditViewModel>(user);
                userModel.Roles = _mapper.Map<List<RoleViewModel>>(userRoles);
                foreach (var role in userModel.Roles)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        role.IsChecked = true;
                    }
                }
            }
            return userModel;
        }
        public async Task<IdentityResult> EditAccount(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.Convert(model);
                if (model.NewPassword != null)
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {
                        return changePasswordResult;
                    }
                }
                var result = await _userManager.UpdateAsync(user);
                foreach (var role in model.Roles)
                {
                    if ((bool)role.IsChecked)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }
                return result;
            }
            else return IdentityResult.Failed(new IdentityError { Code = "404", Description = "Пользователь не найден" });
        }

        public async Task<IdentityResult> EditAccountFromApi(UserEditApiModel model)
        {
            var viewModel = _mapper.Map<UserEditViewModel>(model);
            var roles = new List<RoleViewModel>();
            foreach(var role in model.Roles)
            {
                var roleFromBD = await _roleManager.FindByNameAsync(role);
                if (roleFromBD != null)
                {
                    var roleToModel = _mapper.Map<RoleViewModel>(roleFromBD);
                    roleToModel.IsChecked = true;
                    roles.Add(roleToModel);
                }
            }
            viewModel.Roles = roles;
            return await EditAccount(viewModel);
        }

        public async Task<UserViewResponse> GetAccount(string id)
        {
            var user =  await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var userView = _mapper.Map<UserViewResponse>(user);
            userView.Roles = (List<string>)roles;
            return userView;
        }

        public async Task<UserViewModel> UserPage(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var model = new UserViewModel(user);
            return model;
        }

        public async Task<List<User>> GetAccounts()
        {
            var accounts = _userManager.Users.Select(u => u).ToList();
            return accounts;
        }

        public async Task<AllUsersResponse> GetAccountsResponse()
        {
            var accounts = _userManager.Users.Select(u => u).ToList();
            var usersView = new List<UserViewResponse>();
            foreach(var user in accounts)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userView = _mapper.Map<UserViewResponse>(user);
                userView.Roles = (List<string>)roles;
                usersView.Add(userView);
            }
            var response = new AllUsersResponse
            {
                UsersAmount = accounts.Count,
                Users = usersView
            };
            return response;
        }

        public async Task<IdentityResult> Register(UserRegisterViewModel model)
        {
            var user = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(user, model.PasswordReg);
            if (result.Succeeded)
            {
                var currentUser = await _userManager.FindByIdAsync(user.Id);
                var userRole = await _roleManager.FindByNameAsync("Пользователь");
                await _userManager.AddToRoleAsync(currentUser, userRole.Name);
                await _signInManager.SignInAsync(user, false);
            }
            return result;
        }

        public async Task<IdentityResult> RemoveAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return IdentityResult.Failed();
            }
            if(_httpContextAccessor.HttpContext?.User.Identity.Name == user.UserName )
            {
                _signInManager.SignOutAsync();
            }
            return await _userManager.DeleteAsync(user);
        }

        private async Task AddPostsClaim(User user, List<string> postsIdsToClaim)
        {
            var posts = await _postRep.GetAll();
            foreach (var post in posts)
            {
                await _postRep.LoadAllNavigationPropertiesAsync(post);
            }
            var postsClaim = _userManager.GetClaimsAsync(user).Result.Where(c => c.Type == "Post").ToList();
            foreach (var post in postsIdsToClaim)
            {
                var postClaim = new Claim("Post", post);
                if (!postsClaim.Any(c => c.Value == postClaim.Value))
                {
                    await _userManager.AddClaimAsync(user, postClaim);
                }
            }
        }

        public async Task AddPostClaim(User user, string postIdToClaim)
        {
            await AddPostsClaim(user, new List<string> { postIdToClaim });
        }

        private async Task AddCommentsClaim(User user, List<string> commentsIdsToClaim)
        {
            var comments = await _commentRep.GetAll();
            foreach (var comment in comments)
            {
                await _commentRep.LoadAllNavigationPropertiesAsync(comment);
            }
            var commentsClaim = _userManager.GetClaimsAsync(user).Result.Where(c => c.Type == "Comment").ToList();
            foreach (var comment in commentsIdsToClaim)
            {
                var commentClaim = new Claim("Comment", comment);
                if (!commentsClaim.Any(c => c.Value == commentClaim.Value))
                {
                    await _userManager.AddClaimAsync(user, commentClaim);
                }
            }
        }

        public async Task AddCommentClaim(User user, string commentIdToClaim)
        {
            await AddCommentsClaim(user, new List<string> { commentIdToClaim });
        }

        public async Task GenerateData()
        {
            var testUser1 = new UserRegisterViewModel { Email = "q@gmail.com", PasswordReg = "12345", PasswordConfirm = "12345", FirstName = "Ivan", LastName = "I" };
            var testUser2 = new UserRegisterViewModel { Email = "qw@gmail.com", PasswordReg = "12345", PasswordConfirm = "12345", FirstName = "Oleg", LastName = "O" };
            var testUser3 = new UserRegisterViewModel { Email = "qwerty@gmail.com", PasswordReg = "12345", PasswordConfirm = "12345", FirstName = "Vasya", LastName = "V" };

            var user1 = _mapper.Map<User>(testUser1);
            var user2 = _mapper.Map<User>(testUser2);
            var user3 = _mapper.Map<User>(testUser3);


            var userRole = new Role() { Name = "Пользователь", Description = "Стандартная роль" };
            var moderRole = new Role() { Name = "Модератор", Description = "Права на редактирование статей и комментариев" };
            var adminRole = new Role() { Name = "Администратор", Description = "Максимальные права" };

            var startUsers = new Dictionary<User, string>
            {
                {user1, testUser1.PasswordReg },
                {user2, testUser2.PasswordReg },
                {user3, testUser3.PasswordReg }
            };

            var startRoles = new List<Role> { userRole, moderRole, adminRole };

            foreach( var role in startRoles )
            {
                if(!_roleManager.Roles.Any(r => r.Name == role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }
            }

            foreach (var user in startUsers)
            {
                if (!_userManager.Users.Any(u => u.UserName == user.Key.UserName))
                {
                    await _userManager.CreateAsync(user.Key, user.Value);
                }
            }

            await _userManager.AddToRoleAsync(user1, "Администратор");
            await _userManager.AddToRoleAsync(user2, "Модератор");
            await _userManager.AddToRoleAsync(user3, "Пользователь");
        }

    }
}
