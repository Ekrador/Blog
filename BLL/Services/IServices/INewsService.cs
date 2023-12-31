﻿using BLL.Contracts.Responses;
using BLL.Models.News;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.IServices
{
    public interface INewsService
    {
        Task<string> AddNews(AddNewsViewModel model);
        AddNewsViewModel AddNews();
        Task<EditNewsViewModel> EditNews(string id);
        Task<bool> EditNews(EditNewsViewModel model);
        Task<bool> RemoveNews(string id);
        Task<List<News>> AllNews();
        Task<AllNewsResponse> AllNewsResponse();
    }
}
