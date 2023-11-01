using BLL.Models.News;
using BLL.Models.Posts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class NewsFromModelExt
    {
        public static News Convert(this News news, EditNewsViewModel newseditvm)
        {
            news.Content = newseditvm.Content ?? news.Content;
            news.Title = newseditvm.Title ?? news.Title;
            return news;
        }
    }
}
