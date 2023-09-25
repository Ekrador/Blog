using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Post
    {
        public string Id { get; set; } 
        public DateTime CreationDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        public List<Tag> Tags { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public Post() 
        { 
            Id = Guid.NewGuid().ToString();
            CreationDate = DateTime.Now;
        }
    }
}
