using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts.Responses
{
    public class AllPostsResponse
    {
        public int PostsAmount { get; set; }
        public List<PostViewResponse> Posts { get; set; }
    }
    public class PostViewResponse
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public List<string> Tags { get; set; }
        public List<string> CommentsIds { get; set; }
        public int ViewCount { get; set; }
    }
}
