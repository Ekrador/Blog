using AutoMapper;
using BLL.Models.News;
using BLL.Models.Posts;
using BLL.Services.IServices;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _newsRep;
        public NewsService(IRepository<News> newsRep)
        {
            _newsRep = newsRep;
        }

        public async Task<List<News>> AllNews()
        {
            var news = await _newsRep.GetAll();
            return news.ToList();
        }

        public async Task<bool> AddNews(AddNewsViewModel model)
        {
            var news = new News { Content = model.Content, Title = model.Title };
            return await _newsRep.Create(news);
        }

        public AddNewsViewModel AddNews()
        {
            var model = new AddNewsViewModel();
            return model;
        }

        public async Task<EditNewsViewModel> EditNews(string id)
        {
            var news = await _newsRep.Get(id);
            var model = new EditNewsViewModel{ Id = id, Title = news.Title, Content = news.Content };
            return model;
        }

        public async Task<bool> EditNews(EditNewsViewModel model)
        {
            var news = await _newsRep.Get(model.Id);
            news.Title = model.Title;
            news.Content = model.Content;
            return await _newsRep.Update(news);
        }

        public async Task<bool> RemoveNews(string id)
        {
            var news = await _newsRep.Get(id);
            return await _newsRep.Delete(news);
        }
    }
}
