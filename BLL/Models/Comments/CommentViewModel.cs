using BLL.Models.Posts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Comments
{
    public class CommentViewModel
    {
        public string Id { get; set; }
        public User Author { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public PostViewModel Post { get; set; }
    }
}
