using BLL.Models.Comments;
using BLL.Models.Tags;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Posts
{
    public class PostViewModel
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string AuthorId { get; set; }
        public List<Tag>? Tags { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Comment> Comments { get; set; }
        public CreateCommentViewModel CommentViewModel { get; set; }
    }
}
