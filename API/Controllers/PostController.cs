using BLL.Contracts.Responses;
using BLL.Models.Posts;
using BLL.Services.IServices;
using Blog;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер статей
    /// </summary>
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;
        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        /// <summary>
        /// Создать новую статью
        /// </summary>
        /// <remarks>Требуется вход в систему</remarks>
        /// <param name="model">Данные в формате JSON</param>
        [Authorize]
        [HttpPost]
        [Route("API/Post/Create")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostApiModel model)
        {
            if (ModelState.IsValid)
            {
                model.AuthorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var post = await _postService.CreatePostApi(model);
                if (post != null)
                {
                    _logger.LogInformation($"the user wrote a new post {post}");
                    return StatusCode(200, $"Добавлена статья {post}");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return BadRequest(model);
        }

        /// <summary>
        /// Редактировать статью
        /// </summary>
        /// <remarks>Требуется вход в систему под автором статьи или администратором/модератором</remarks>
        /// <param name="id">Id статьи</param>
        /// <param name="model">Данные в формате JSON</param>

        [AuthorizationEditPost]
        [HttpPatch]
        [Route("API/Post/Edit/{id}")]
        public async Task<IActionResult> EditPost([FromRoute] string id, [FromBody] EditPostApiModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = id;
                var result = await _postService.EditPostFromApi(model);
                if (result)
                {
                    _logger.LogInformation($"the user {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} edited post {model.Id}");
                    return StatusCode(200, $"Пользователем {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} изменена статья {model.Id}");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return BadRequest(model);
        }

        /// <summary>
        /// Удалить статью
        /// </summary>
        /// <remarks>Требуется вход в систему под автором статьи или администратором/модератором</remarks>
        /// <param name="id">Id статьи</param>
        [HttpDelete]
        [AuthorizationEditPost]
        [Route("API/Post/RemovePost/{id}")]
        public async Task<IActionResult> RemovePost([FromRoute] string id)
        {
            var result = await _postService.RemovePost(id);
            if (result)
            {
                _logger.LogWarning($"the user {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} deleted post {id}");
                return StatusCode(200, $"Пользователем {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} удалена статья {id}");
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Список всех сатей
        /// </summary>
        [HttpGet]
        [Route("API/Post/AllPosts")]
        public async Task<AllPostsResponse> AllPosts()
        {
            var posts = await _postService.GetAllPostsResponse();
            return posts;
        }

        /// <summary>
        /// Список всех сатей пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        [HttpGet]
        [Route("API/Post/PostsByAuthor/{id}")]
        public async Task<AllPostsResponse> PostsByAuthor([FromRoute] string id)
        {
            var posts = await _postService.GetPostsByAuthorResponse(id);
            return posts;
        }

        /// <summary>
        /// Информация о статье 
        /// </summary>
        /// <param name="id">Id статьи</param>
        [HttpGet]
        [Route("API/Post/View/{id}")]
        public async Task<PostViewResponse> ViewPost([FromRoute] string id)
        {
            var post = await _postService.ViewPostResponse(id);
            return post;
        }

        /// <summary>
        /// Список всех сатей с тегом
        /// </summary>
        /// <param name="id">Id тега</param>
        [HttpGet]
        [Route("API/Post/ByTag/{id}")]
        public async Task<AllPostsResponse> PostsByTag([FromRoute] string id)
        {
            var posts = await _postService.GetPostsByTagResponse(id);
            return posts;
        }
    }
}
