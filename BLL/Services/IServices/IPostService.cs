using BLL.Models.Post;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.IServices
{
    public interface IPostService
    {
        Task<bool> CreatePost(CreatePostViewModel model);

        Task<bool> EditPost(EditPostViewModel model);

        Task<bool> RemovePost(string id);

        Task<List<Post>> GetAllPosts();

        Task<List<Post>> GetAuthorsPosts(string authorId);
    }
}
