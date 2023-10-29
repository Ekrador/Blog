using BLL.Models.Comments;
using BLL.Services.IServices;
using Blog;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CommentController> _logger;
        public CommentController(ICommentService commentService, UserManager<User> userManager, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("API/Comment/Create")]
        public async Task<IActionResult> WriteComment([FromBody] CreateCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = await _commentService.WriteComment(model);
                if (comment)
                {
                    _logger.LogInformation($"the user wrote a new comment on post {model.PostId}");

                    return StatusCode(200);
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return StatusCode(400);
        }

        [AuthorizationEditComment]
        [HttpPatch]
        [Route("API/Comment/Edit")]
        public async Task<IActionResult> EditComment([FromBody] EditCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.EditComment(model);
                if (result)
                {
                    _logger.LogInformation($"the user edit comment {model.Id}");
                    return StatusCode(200);
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return StatusCode(400);
        }

        [HttpDelete]
        [AuthorizationEditComment]
        [Route("API/Comment/RemoveComment/{id}")]
        public async Task<IActionResult> RemoveComment([FromRoute] string id)
        {
            var result = await _commentService.RemoveComment(id);
            if (result)
            {
                _logger.LogWarning($"the user delete comment {id}");
                return StatusCode(200);
            }
            else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        [Route("API/Comment/AllComments")]
        public async Task<List<Comment>> AllComments()
        {
            var comments = await _commentService.GetAllComments();
            return comments;
        }

        [HttpGet]
        [Route("API/Comment/CommentsByAuthor")]
        public async Task<CommentsByAuthorViewModel> CommentsByAuthor([FromBody] string id)
        {
            var comments = await _commentService.GetCommentsByAuthor(id);

            return comments;
        }

        [HttpGet]
        [Route("API/Comment/ViewComment")]
        public async Task<CommentViewModel> ViewComment([FromBody] string id)
        {
            var comment = await _commentService.ViewComment(id);

            return comment;
        }
    }
}
