using BLL.Models.Tags;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;
        public TagController(ITagService tagService, ILogger<TagController> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        [Route("API/Tag/Create")]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = await _tagService.CreateTag(model);
                if (tag)
                {
                    _logger.LogInformation("new tag created");
                    return StatusCode(200);
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return StatusCode(400);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPatch]
        [Route("API/Tag/Edit")]
        public async Task<IActionResult> EditTag([FromBody] EditTagViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagService.EditTag(model);
                if (result)
                {
                    _logger.LogInformation($"tag edited: {model.Id}");
                    return StatusCode(200);
                }
                else
                {
                    ModelState.AddModelError("", "Некорректное имя тега");
                }
            }
            ModelState.AddModelError("", "Некорректные данные");
            return StatusCode(404);
        }

        [HttpDelete]
        [Authorize(Roles = "Администратор, Модератор")]
        [Route("API/Tag/RemoveTag")]
        public async Task<IActionResult> RemoveTag([FromBody] string id)
        {
            var result = await _tagService.RemoveTag(id);
            if (result)
            {
                _logger.LogWarning($"tag deleted: {id}");
                return StatusCode(200);
            }
            else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        [Route("API/Tag/AllTags")]
        public async Task<List<Tag>> AllTags()
        {
            var tags = await _tagService.GetAllTags();

            return tags;
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        [Route("API/Tag/GetTag/{id}")]
        public async Task<Tag> GetTag([FromRoute] string id)
        {
            var tag = await _tagService.GetTag(id);

            return tag;
        }
    }
}
