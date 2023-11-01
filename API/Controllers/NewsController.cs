using BLL.Contracts.Responses;
using BLL.Models.News;
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
    /// Контроллер новостей
    /// </summary>
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<NewsController> _logger;
        public NewsController(INewsService newsService, UserManager<User> userManager, ILogger<NewsController> logger)
        {
            _newsService = newsService;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Добавить новость
        /// </summary>
        /// <remarks>Требуется вход в систему под администратором/модератором</remarks>
        /// <param name="model">данные  в формате JSON</param>
        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        [Route("API/News/Add")]
        public async Task<IActionResult> AddNews([FromBody] AddNewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var news = await _newsService.AddNews(model);
                if (news != null)
                {
                    _logger.LogInformation($"the user added new news {news}");
                    return StatusCode(200, $"Добавлена новость {news}");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return BadRequest(model);
        }

        /// <summary>
        /// Редактировать новость
        /// </summary>
        /// <remarks>Требуется вход в систему под администратором/модератором</remarks>
        /// <param name="id">Id новости</param>
        /// <param name="model">Данные  в формате JSON</param>
        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPatch]
        [Route("API/News/Edit/{id}")]
        public async Task<IActionResult> EditNews([FromRoute] string id,[FromBody] EditNewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = id;
                var result = await _newsService.EditNews(model);
                if (result)
                {
                    _logger.LogInformation($"the user {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} edited news {model.Id}");
                    return StatusCode(200, $"Пользователем {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} изменена новость {model.Id}");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return BadRequest(model);
        }

        /// <summary>
        /// Удалить новость
        /// </summary>
        /// <remarks>Требуется вход в систему под администратором/модератором</remarks>
        /// <param name="id">Id новости</param>
        [HttpDelete]
        [Authorize(Roles = "Администратор, Модератор")]
        [Route("API/News/RemoveNews/{id}")]
        public async Task<IActionResult> RemoveNews([FromRoute] string id)
        {
            var result = await _newsService.RemoveNews(id);
            if (result)
            {
                _logger.LogWarning($"the user {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} deleted news {id}");
                return StatusCode(200, $"Пользователем {User.FindFirst(ClaimTypes.NameIdentifier)?.Value} удалена новость {id}");
            }
            else
            {
                return StatusCode(404);
            }
        }

        /// <summary>
        /// Список новостей
        /// </summary>
        [HttpGet]
        [Route("API/News/AllNews")]
        public async Task<AllNewsResponse> AllNews()
        {
            var news = await _newsService.AllNewsResponse();
            return news;
        }
    }
}
