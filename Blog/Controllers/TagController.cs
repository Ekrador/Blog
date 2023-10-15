using BLL.Models.Comments;
using BLL.Models.Tags;
using BLL.Services;
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
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;
        public TagController(ITagService tagService, ILogger<TagController> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        [HttpGet]
        [Route("Tag/Create")]
        public async Task<IActionResult> CreateTag()
        {
            return View(new CreateTagViewModel());
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        [Route("Tag/Create")]
        public async Task<IActionResult> CreateTag(CreateTagViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = await _tagService.CreateTag(model);
                if (tag)
                {
                    _logger.LogInformation("new tag created");
                    return RedirectToAction("AllTags", "Tag");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return View("CreateTag", model);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        [Route("Tag/Edit")]
        public async Task<IActionResult> EditTag(string id)
        {
            var model = await _tagService.EditTag(id);
            if (model == null)
                return StatusCode(404);
            return View(model);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        [Route("Tag/Edit")]
        public async Task<IActionResult> EditTag(EditTagViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagService.EditTag(model);
                if (result)
                {
                    _logger.LogInformation($"tag edited: {model.Id}");
                    return RedirectToAction("AllTags", "Tag");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректное имя тега");
                }
            }
            ModelState.AddModelError("", "Некорректные данные");
            return View("Edit", model);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Модератор")]
        [Route("Tag/RemoveTag")]
        public async Task<IActionResult> RemoveTag(string id)
        {
            await _tagService.RemoveTag(id);
            _logger.LogWarning($"tag deleted: {id}");
            return RedirectToAction("AllTags", "Tag");
        }

        [HttpGet]
        [Route("Tag/AllTags")]
        public async Task<IActionResult> AllTags()
        {
            var tags = await _tagService.GetAllTags();

            return View( new AllTagsViewModel { Tags = tags});
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        [Route("Tag/GetTag/{id}")]
        public async Task<Tag> GetTag([FromRoute] string id)
        {
            var tag = _tagService.GetTag(id);

            return await Task.FromResult(tag.Result);
        }
    }
}
