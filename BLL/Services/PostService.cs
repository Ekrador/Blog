using AutoMapper;
using BLL.Extensions;
using BLL.Models.Comments;
using BLL.Models.Posts;
using BLL.Models.Roles;
using BLL.Models.Tags;
using BLL.Services.IServices;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> _postRep;
        private readonly IRepository<Tag> _tagRep;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRepository<Comment> _commentRep;
        private readonly IUserService _userService;
        public PostService(IRepository<Post> repository, IMapper mapper, IRepository<Tag> tagRep, UserManager<User> userManager,
            IRepository<Comment> commentRep, IUserService userService, SignInManager<User> signInManager)
        {
            _postRep = repository;
            _mapper = mapper;
            _tagRep = tagRep;
            _userManager = userManager;
            _commentRep = commentRep;
            _userService = userService;
            _signInManager = signInManager;
        }
        public async Task<CreatePostViewModel> CreatePost()
        {
            var allTags = await _tagRep.GetAll();
            var tags = allTags?.Select(t => new TagViewModel() { TagId = t.Id, Name = t.Name }).ToList();
            var model = new CreatePostViewModel
            {
                Tags = tags
            };
            return model;
        }

        public async Task<string> CreatePost(CreatePostViewModel model)
        {
            List<Tag> selectedTags = new();
            if (model.Tags != null)
            {
                var _postTags = model.Tags.Where(t => t.IsChecked == true).ToList();
                var tagsId = _postTags.Select(t => t.TagId).ToList();
                var dbTags = await _tagRep.GetAll();
                selectedTags = dbTags.Where(t => tagsId.Contains(t.Id)).ToList();
            }
            var post = _mapper.Map<Post>(model);
            post.Tags = selectedTags;
            post.Author = _userManager.FindByIdAsync(model.AuthorId).Result;
            var result = await _postRep.Create(post);
            if (result)
            {
                await _userService.AddPostClaim(post.Author, post.Id);
                await _signInManager.SignInAsync(post.Author, true);
                return post.Id;
            }
            return null;
        }

        public async Task<EditPostViewModel> EditPost(string id)
        {
            EditPostViewModel postModel = null;
            var post = await _postRep.Get(id);
            if (post != null)
            {
                await _postRep.LoadAllNavigationPropertiesAsync(post);
                var tags = post.Tags;
                var allTags = await _tagRep.GetAll();
                postModel = _mapper.Map<EditPostViewModel>(post);
                postModel.Tags = _mapper.Map<List<TagViewModel>>(allTags.ToList()) ?? new List<TagViewModel>();
                foreach (var tag in postModel.Tags)
                {
                    if (tags.Any(t => t.Id == tag.TagId))
                    {
                        tag.IsChecked = true;
                    }
                }
            }
            return postModel;
        }

        public async Task<bool> EditPost(EditPostViewModel model)
        {
            var post = await _postRep.Get(model.Id);
            if (post != null)
            {
                await _postRep.LoadAllNavigationPropertiesAsync(post);
            }
            foreach (var tag in model.Tags)
            {
                var tagChanged = await _tagRep.Get(tag.TagId);
                if (tag.IsChecked && (post.Tags.Where(p => p.Id == tag.TagId).FirstOrDefault() == null))
                {
                    post.Tags.Add(tagChanged);
                }
                else if (!tag.IsChecked && post.Tags.Where(p => p.Id == tag.TagId).FirstOrDefault() != null)
                {
                    post.Tags.Remove(tagChanged);
                }
            }
            post?.Convert(model);
            return await _postRep.Update(post);
        }

        public async Task<List<Post>> GetAllPosts()
        {
            var posts = await _postRep.GetAll();
            if (posts != null)
            {
                foreach (var post in posts)
                    await _postRep.LoadAllNavigationPropertiesAsync(post);
            }
            return posts?.OrderByDescending(с => с.ViewCount).ToList();
        }

        public async Task<List<Post>> GetAuthorsPosts(string authorId)
        {
            var posts = await _postRep.GetAll();
            var authorPosts = posts.Where(p => p.Author.Id == authorId);
            return authorPosts.ToList();
        }

        public async Task<PostViewModel> ViewPost(string id)
        {
            var post = await _postRep.Get(id);
            if (post != null)
            {
                post.ViewCount++;
                await _postRep.Update(post);
                await _postRep.LoadAllNavigationPropertiesAsync(post);
                foreach (var comment in post.Comments)
                {
                    await _commentRep.LoadAllNavigationPropertiesAsync(comment);
                }
            }
            var model = _mapper.Map<PostViewModel>(post);
            model.AuthorId = post?.Author.Id;
            return model;
        }

        public async Task<bool> RemovePost(string id)
        {
            var post = await _postRep.Get(id);
            return await _postRep.Delete(post);
        }

        public async Task<PostsByTagViewModel> GetPostsByTag(string id)
        {
            var tag = await _tagRep.Get(id);
            var allPosts = await _postRep.GetAll();
            foreach (var post in allPosts)
            {
                await _postRep.LoadAllNavigationPropertiesAsync(post);
            }
            var posts = allPosts.Where(p => p.Tags.Contains(tag)).OrderByDescending(с => с.ViewCount).ToList();
            var model = new PostsByTagViewModel { Posts = posts, TagName = tag.Name };
            return model;
        }

        public async Task<PostsByAuthorViewModel> GetPostsByAuthor(string authorId)
        {
            var allPosts = await _postRep.GetAll();
            foreach (var post in allPosts)
            {
                await _postRep.LoadAllNavigationPropertiesAsync(post);
            }
            var posts = allPosts.Where(p => p.Author?.Id == authorId).OrderByDescending(с => с.ViewCount).ToList();
            var model = new PostsByAuthorViewModel { Posts = posts, AuthorName = _userManager.FindByIdAsync(authorId).Result.GetFullName() };
            return model;
        }

        public async Task<Post> GetPostById(string id)
        {
            var post = await _postRep.Get(id);
            return post;
        }
    }
}
