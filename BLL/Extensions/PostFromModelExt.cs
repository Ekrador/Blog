using BLL.Models.Post;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class PostFromModelExt
    {
        public static Post Convert(this Post post, EditPostViewModel posteditvm)
        {
            post.Tags = posteditvm.Tags;
            post.Content = posteditvm.Content ?? post.Content;
            post.Title = posteditvm.Title ?? post.Title;
            return post;
        }
    }
}
