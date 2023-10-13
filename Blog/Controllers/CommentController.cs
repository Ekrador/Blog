using BLL.Models.Comments;
using BLL.Models.Posts;
using BLL.Models.Tags;
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
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        public CommentController(ICommentService commentService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _userManager = userManager; 
        }

        [Authorize]
        [HttpPost]
        [Route("Comment/Create")]
        public async Task<IActionResult> WriteComment(CreateCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = await _commentService.WriteComment(model);
                if (comment)
                {
                    return RedirectToAction("ViewPost", "Post", new { Id = model.PostId });
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return View("_partialAddComment", model);
        }

        [AuthorizationEditComment]
        [HttpGet]
        [Route("Comment/Edit/{id}")]
        public async Task<IActionResult> EditComment(string id)
        {
            var model = await _commentService.EditComment(id);
            if (model == null)
                return StatusCode(404);
            return View(model);
        }

        [AuthorizationEditComment]
        [HttpPost]
        [Route("Comment/Edit/{id}")]
        public async Task<IActionResult> EditComment(EditCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.EditComment(model);
                if (result)
                {
                    return RedirectToAction("AllComments", "Comment");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return View("EditComment", model);
        }

        [HttpPost]
        [AuthorizationEditComment]
        [Route("Comment/RemoveComment/{id}")]
        public async Task<IActionResult> RemoveComment(string id)
        {
            await _commentService.RemoveComment(id);
            var comments = await _commentService.GetAllComments();
            return View("AllComments", new AllCommentsViewModel { Comments = comments });
        }

        [HttpGet]
        [Route("Comment/AllComments")]
        public async Task<IActionResult> AllComments()
        {
            var comments = await _commentService.GetAllComments();
            return View(new AllCommentsViewModel { Comments = comments });
        }

        [HttpGet]
        [Route("Comment/CommentsByAuthor")]
        public async Task<IActionResult> CommentsByAuthor(string id)
        {
            var comments = await _commentService.GetCommentsByAuthor(id);

            return View(comments);
        }

        [HttpGet]
        [Route("Comment/ViewComment")]
        public async Task<IActionResult> ViewComment(string id)
        {
            var comment = await _commentService.ViewComment(id);

            return View(comment);
        }
    }
}
