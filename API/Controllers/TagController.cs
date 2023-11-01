using BLL.Contracts.Responses;
using BLL.Models.Tags;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер тегов
    /// </summary>
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;
        public TagController(ITagService tagService, ILogger<TagController> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        /// <summary>
        /// Создать новый тег
        /// </summary>
        /// <remarks>Требуется вход в систему под администратором/модератором</remarks>
        /// <param name="model">Данные в формате JSON</param>
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
                    return StatusCode(200, $"Создан тег {model.Name}");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return BadRequest(model);
        }

        /// <summary>
        /// Редактировать тег
        /// </summary>
        /// <remarks>Требуется вход в систему под администратором/модератором</remarks>
        /// <param name="model">Данные в формате JSON</param>
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
                    return StatusCode(200, $"Тег {model.Id} изменен пользователем {User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректное имя тега");
                }
            }
            ModelState.AddModelError("", "Некорректные данные");
            return BadRequest(model);
        }

        /// <summary>
        /// Удалить тег
        /// </summary>
        /// <remarks>Требуется вход в систему под администратором/модератором</remarks>
        /// <param name="id">Id тега</param>
        [HttpDelete]
        [Authorize(Roles = "Администратор, Модератор")]
        [Route("API/Tag/RemoveTag/{id}")]
        public async Task<IActionResult> RemoveTag([FromRoute] string id)
        {
            var result = await _tagService.RemoveTag(id);
            if (result)
            {
                _logger.LogWarning($"tag deleted: {id}");
                return StatusCode(200, $"Тег {id} удален пользователем {User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Список тегов
        /// </summary>
        [HttpGet]
        [Route("API/Tag/AllTags")]
        public async Task<AllTagsResponse> AllTags()
        {
            var tags = await _tagService.GetAllTagsResponse();

            return tags;
        }

        /// <summary>
        /// Информация о теге
        /// </summary>
        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        [Route("API/Tag/GetTag/{id}")]
        public async Task<TagViewResponse> GetTag([FromRoute] string id)
        {
            var tag = await _tagService.GetTagResponse(id);
            return tag;
        }
    }
}
