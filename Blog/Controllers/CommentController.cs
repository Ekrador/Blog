using BLL.Models.Comments;
using BLL.Models.Post;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    internal class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> WriteComment(CreateCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = await _commentService.WriteComment(model);
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
        public async Task<IActionResult> Edit(EditCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.EditComment(model);
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
        [Route("RemoveComment/{id}")]
        public async Task<IActionResult> RemoveComment([FromRoute] string id)
        {
            await _commentService.RemoveComment(id);
            return StatusCode(200);
        }

        [HttpGet]
        [Route("AllComments")]
        public async Task<List<Comment>> GetPosts()
        {
            var comment = await _commentService.GetAllComments();

            return await Task.FromResult(comment);
        }

        [HttpGet]
        [Route("GetComment/{id}")]
        public async Task<Comment> GetComment([FromRoute] string id)
        {
            var comment = _commentService.GetComment(id);

            return await Task.FromResult(comment.Result);
        }
    }
}
