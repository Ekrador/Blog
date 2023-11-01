using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts.Responses
{
    public class AllCommentsResponse
    {
        public int CommentsAmount { get; set; }
        public List<CommentViewResponse> Comments { get; set; }
    }
    public class CommentViewResponse
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; } 
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public string PostId { get; set; }
    }
}
