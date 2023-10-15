using AutoMapper;
using BLL.Extensions;
using BLL.Models.Comments;
using BLL.Models.Posts;
using BLL.Models.Tags;
using BLL.Services.IServices;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentRep;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRepository<Post> _postRep;
        private readonly IUserService _userService;
        public CommentService(IRepository<Comment> commentRep, IMapper mapper, UserManager<User> userManager, IRepository<Post> postRep, IUserService userService, SignInManager<User> signinManager)
        {
            _commentRep = commentRep;
            _mapper = mapper;
            _userManager = userManager;
            _postRep = postRep;
            _userService = userService;
            _signInManager = signinManager;
        }

        public async Task<EditCommentViewModel> EditComment(string id)
        {
            EditCommentViewModel model = null;
            var comment = await _commentRep.Get(id);
            if (comment != null)
            {
                model = new()
                {
                    Id = comment.Id,
                    Content = comment.Content
                };
            }
            return model;
        }

        public async Task<bool> EditComment(EditCommentViewModel model)
        {
            var comment = await _commentRep.Get(model.Id);
            comment.Content = model.Content;
            return await _commentRep.Update(comment);
        }

        public async Task<List<Comment>> GetAllComments()
        {
            var comments = await _commentRep.GetAll();
            foreach (var comment in comments)
            {
                await _commentRep.LoadAllNavigationPropertiesAsync(comment);
            }
            return comments.OrderByDescending(c => c.CreationDate).ToList();
        }

        public async Task<CommentsByAuthorViewModel> GetCommentsByAuthor(string authorId)
        {
            var allComments = await _commentRep.GetAll();
            foreach (var comment in allComments)
            {
                await _commentRep.LoadAllNavigationPropertiesAsync(comment);
            }
            var comments = allComments.Where(c => c.Author?.Id == authorId).OrderByDescending(с => с.CreationDate).ToList();
            var model = new CommentsByAuthorViewModel { Comments = comments, AuthorName = _userManager.FindByIdAsync(authorId).Result.GetFullName() };
            return model;
        }

        public async Task<bool> RemoveComment(string id)
        {
            var post = await _commentRep.Get(id);
            return await _commentRep.Delete(post);
        }

        public async Task<CommentViewModel> ViewComment(string id)
        {
            var comment = await _commentRep.Get(id);
            if (comment != null)
            {
                await _commentRep.LoadAllNavigationPropertiesAsync(comment);
            }
            var post = await _postRep.Get(comment?.Post.Id);
            var postModel = _mapper.Map<PostViewModel>(post);
            var model = _mapper.Map<CommentViewModel>(comment);
            model.Post = postModel;
            return model;
        }

        public async Task<bool> WriteComment(CreateCommentViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.AuthorId);
            var post = await _postRep.Get(model.PostId);
            var comment = new Comment { Author = user, Content = model.Content, Post = post };
            var result = await _commentRep.Create(comment);
            if (result)
            {
                await _userService.AddCommentClaim(user, comment.Id);
                await _signInManager.SignInAsync(user, true);

            }
            return result;
        }
    }
}
