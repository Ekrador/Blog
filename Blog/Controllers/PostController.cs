using BLL.Models;
using BLL.Models.Post;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    internal class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly UserManager<User> _userManager;
        public PostController(IPostService postService, UserManager<User> userManager)
        {
            _postService = postService;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreatePost(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = User;
                var result = await _userManager.GetUserAsync(user);
                model.AuthorId = result.Id;
                var post = await _postService.CreatePost(model);
                if (post)
                {
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            ModelState.AddModelError("", "Некорректные данные");
            return StatusCode(400);
        }

        [Authorize]
        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> Edit(EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _postService.EditPost(model);
                if (result)
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

        [HttpDelete]
        [Authorize(Roles = "Администратор")]
        [Route("RemovePost/{id}")]
        public async Task<IActionResult> RemovePost([FromRoute] string id)
        {
            await _postService.RemovePost(id);
            return StatusCode(200);
        }

        [HttpGet]
        [Route("AllPosts")]
        public async Task<List<Post>> GetPosts()
        {
            var posts = await _postService.GetAllPosts();

            return await Task.FromResult(posts);
        }

        [HttpGet]
        [Route("GetAuthorsPosts/{id}")]
        public async Task<List<Post>> GetAuthorsPosts([FromRoute] string id)
        {
            var posts = _postService.GetAuthorsPosts(id);

            return await Task.FromResult(posts.Result);
        }
    }
}
