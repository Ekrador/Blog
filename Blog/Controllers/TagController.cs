using BLL.Models.Comments;
using BLL.Models.Tags;
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
    internal class TagController : Controller
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateTag(CreateTagViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = await _tagService.CreateTag(model);
                if (tag)
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

        [Authorize(Roles = "Администратор")]
        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> Edit(EditTagViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = await _tagService.EditTag(model);
                if (tag)
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
        [Route("RemoveTag/{id}")]
        public async Task<IActionResult> RemoveTag([FromRoute] string id)
        {
            await _tagService.RemoveTag(id);
            return StatusCode(200);
        }

        [HttpGet]
        [Route("AllTags")]
        public async Task<List<Tag>> GetTags()
        {
            var tag = await _tagService.GetAllTags();

            return await Task.FromResult(tag);
        }

        [HttpGet]
        [Route("GetTag/{id}")]
        public async Task<Tag> GetTag([FromRoute] string id)
        {
            var tag = _tagService.GetTag(id);

            return await Task.FromResult(tag.Result);
        }
    }
}
