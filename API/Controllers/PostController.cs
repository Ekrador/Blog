using BLL.Models.Posts;
using BLL.Services.IServices;
using Blog;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PostController : ControllerBase
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
        [HttpPost]
        [Route("API/Post/Create")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = await _postService.CreatePost(model);
                if (post != null)
                {
                    _logger.LogInformation($"the user wrote a new post {post}");
                    return StatusCode(200);
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return StatusCode(400);
        }

        [AuthorizationEditPost]
        [HttpPatch]
        [Route("API/Post/Edit")]
        public async Task<IActionResult> EditPost([FromBody] EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _postService.EditPost(model);
                if (result)
                {
                    _logger.LogInformation($"the user edited post {model.Id}");
                    return StatusCode(200);
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return StatusCode(404);
        }

        [HttpDelete]
        [AuthorizationEditPost]
        [Route("API/Post/RemovePost/{id}")]
        public async Task<IActionResult> RemovePost([FromRoute] string id)
        {
            var result = await _postService.RemovePost(id);
            if (result)
            {
                _logger.LogWarning($"the user deleted post {id}");
                return StatusCode(200);
            }
            else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        [Route("API/Post/AllPosts")]
        public async Task<List<Post>> AllPosts()
        {
            var posts = await _postService.GetAllPosts();
            return posts;
        }

        [HttpGet]
        [Route("API/Post/PostsByAuthor")]
        public async Task<PostsByAuthorViewModel> PostsByAuthor([FromBody] string id)
        {
            var posts = await _postService.GetPostsByAuthor(id);

            return posts;
        }

        [HttpGet]
        [Route("API/Post/View")]
        public async Task<PostViewModel> ViewPost([FromBody] string id)
        {
            var post = await _postService.ViewPost(id);
            return post;
        }

        [HttpGet]
        [Route("API/Post/ByTag")]
        public async Task<PostsByTagViewModel> PostsByTag([FromBody] string id)
        {
            var posts = await _postService.GetPostsByTag(id);
            return posts;
        }
    }
}
