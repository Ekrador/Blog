using AutoMapper;
using BLL.Contracts.Responses;
using BLL.Extensions;
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
using System.Xml.Linq;

namespace BLL.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _newsRep;
        private readonly IMapper _mapper;
        public NewsService(IRepository<News> newsRep, IMapper mapper)
        {
            _newsRep = newsRep;
            _mapper = mapper;
        }

        public async Task<List<News>> AllNews()
        {
            var news = await _newsRep.GetAll();
            return news.ToList();
        }

        public async Task<AllNewsResponse> AllNewsResponse()
        {
            var news = await _newsRep.GetAll();
            var sortedNews = news.OrderByDescending(c => c.CreationDate).ToList();
            var response = new AllNewsResponse
            {
                NewsAmount = news.Count(),
                News = _mapper.Map<List<News>, List<NewsViewResponse>>(sortedNews)
            };
            return response;
        }

        public async Task<string> AddNews(AddNewsViewModel model)
        {
            var news = new News { Content = model.Content, Title = model.Title };
            var result = await _newsRep.Create(news);
            if(result)
            {
                return news.Id;
            }
            return null;
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
            news.Convert(model);
            return await _newsRep.Update(news);
        }

        public async Task<bool> RemoveNews(string id)
        {
            var news = await _newsRep.Get(id);
            return await _newsRep.Delete(news);
        }
    }
}
