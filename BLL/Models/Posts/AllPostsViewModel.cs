using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace BLL.Models.Posts
{
    public class AllPostsViewModel
    {
        public string Info { get; set; }
        public List<Post> Posts { get; set; }
    }
}
