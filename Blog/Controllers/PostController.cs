using BLL.Models;
using BLL.Models.Posts;
using BLL.Services;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PostController> _logger;
        public PostController(IPostService postService, UserManager<User> userManager, ILogger<PostController> logger)
        {
            _postService = postService;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("Post/Create")]
        public async Task<IActionResult> CreatePost()
        {
            var model = await _postService.CreatePost();
            var user = User;
            var result = await _userManager.GetUserAsync(user);
            model.AuthorId = result.Id;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [Route("Post/Create")]
        public async Task<IActionResult> CreatePost(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = await _postService.CreatePost(model);
                if (post != null)
                {
                    _logger.LogInformation($"the user wrote a new post {post}");
                    return RedirectToAction("ViewPost", "Post", new { Id = post });
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return RedirectToAction("CreatePost");
        }

        [AuthorizationEditPost]
        [HttpGet]
        [Route("Post/Edit/{id}")]
        public async Task<IActionResult> EditPost(string id)
        {
            var model = await _postService.EditPost(id);
            if (model == null)
                return StatusCode(404);
            return View(model);
        }

        [AuthorizationEditPost]
        [HttpPost]
        [Route("Post/Edit/{id}")]
        public async Task<IActionResult> EditPost(EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _postService.EditPost(model);
                if (result)
                {
                    _logger.LogInformation($"the user edited post {model.Id}");
                    return RedirectToAction("ViewPost", "Post", new { Id = model.Id });
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return View("EditPost", model);
        }

        [HttpPost]
        [AuthorizationEditPost]
        [Route("Post/RemovePost/{id}")]
        public async Task<IActionResult> RemovePost(string id)
        {
            await _postService.RemovePost(id);
            _logger.LogWarning($"the user deleted post {id}");
            var posts = await _postService.GetAllPosts();

            return View("AllPosts", new AllPostsViewModel { Posts = posts });
        }

        [HttpGet]
        [Route("Post/AllPosts")]
        public async Task<IActionResult> AllPosts()
        {
            var posts = await _postService.GetAllPosts();

            return View(new AllPostsViewModel { Posts = posts });
        }

        [HttpGet]
        [Route("Post/PostsByAuthor")]
        public async Task<IActionResult> PostsByAuthor(string id)
        {
            var posts = await _postService.GetPostsByAuthor(id);

            return View(posts);
        }

        [HttpGet]
        [Route("Post/View")]
        public async Task<IActionResult> ViewPost(string id)
        {
            var post = await _postService.ViewPost(id);
            return View(post);
        }

        [HttpGet]
        [Route("Post/ByTag")]
        public async Task<IActionResult> PostsByTag(string id)
        {
            var posts = await _postService.GetPostsByTag(id);
            return View(posts);
        }
    }
}
