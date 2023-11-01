using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public string Content { get; set; }
        public User Author { get; set; }
        public Post Post { get; set; }
        public Comment() 
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
