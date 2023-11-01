using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts.Responses
{
    public class AllNewsResponse
    {
        public int NewsAmount { get; set; }
        public List<NewsViewResponse> News { get; set; }
    }
    public class NewsViewResponse
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
