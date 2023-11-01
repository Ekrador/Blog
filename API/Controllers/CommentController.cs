using AutoMapper;
using BLL.Contracts.Responses;
using BLL.Models.Comments;
using BLL.Services.IServices;
using Blog;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
    {
    /// <summary>
    /// Контроллер комментариев
    /// </summary>
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CommentController> _logger;
        private readonly IMapper _mapper;
        public CommentController(ICommentService commentService, UserManager<User> userManager, ILogger<CommentController> logger, IMapper mapper)
        {
            _commentService = commentService;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Написать новый комментарий
        /// </summary>
        /// <remarks>Требуется вход в систему</remarks>
        /// <param name="model">Данные  в формате JSON</param>
        [Authorize]
        [HttpPost]
        [Route("API/Comment/Create")]
        public async Task<IActionResult> WriteComment([FromBody] CreateCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.AuthorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var comment = await _commentService.WriteComment(model);
                if (comment)
                {
                    _logger.LogInformation($"the user wrote a new comment on post {model.PostId}");

                    return StatusCode(200, $"Пользователем {model.AuthorId} оставлен комментарий к посту {model.PostId}");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return BadRequest(model);
        }

        /// <summary>
        /// Редактировать комментарий
        /// </summary>
        /// <remarks>Требуется вход в систему под автором комментария или администратором/модератором</remarks>
        /// <param name="id">Id комментария</param>
        /// <param name="model">Данные  в формате JSON</param>
        [AuthorizationEditComment]
        [HttpPatch]
        [Route("API/Comment/Edit/{id}")]
        public async Task<IActionResult> EditComment([FromRoute] string id, [FromBody] EditCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = id;
                var result = await _commentService.EditComment(model);
                if (result)
                {
                    _logger.LogInformation($"the user {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} edit comment {model.Id}");
                    return StatusCode(200, $"Пользователем {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} изменен комментарий {model.Id}");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return BadRequest(model);
        }

        /// <summary>
        /// Удалить комментарий
        /// </summary>
        /// <remarks>Требуется вход в систему под автором комментария или администратором/модератором</remarks>
        /// <param name="id">Id комментария</param>
        [HttpDelete]
        [AuthorizationEditComment]
        [Route("API/Comment/RemoveComment/{id}")]
        public async Task<IActionResult> RemoveComment([FromRoute] string id)
        {
            var result = await _commentService.RemoveComment(id);
            if (result)
            {
                _logger.LogWarning($"the user {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} delete comment {id}");
                return StatusCode(200, $"Пользователем {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} удален комментарий {id}");
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Список всех комментариев
        /// </summary>
        [HttpGet]
        [Route("API/Comment/AllComments")]
        public async Task<AllCommentsResponse> AllComments()
        {
            var comments = await _commentService.GetAllCommentsResponse();
            return comments;
        }

        /// <summary>
        /// Список комментариев пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        [HttpGet]
        [Route("API/Comment/CommentsByAuthor/{id}")]
        public async Task<AllCommentsResponse> CommentsByAuthor([FromRoute] string id)
        {
            var comments = await _commentService.GetCommentsByAuthorResponse(id);
            return comments;
        }

        /// <summary>
        /// Информация о комментарии
        /// </summary>
        /// <param name="id">Id комментария</param>
        [HttpGet]
        [Route("API/Comment/ViewComment/{id}")]
        public async Task<CommentViewResponse> ViewComment([FromRoute] string id)
        {
            var comment = await _commentService.ViewComment(id);
            var commentView = _mapper.Map<CommentViewResponse>(comment);
            return commentView;
        }
    }
}
