using BLL.Models.Posts;
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
        Task<string> CreatePost(CreatePostViewModel model);
        Task<CreatePostViewModel> CreatePost();
        Task<EditPostViewModel> EditPost(string id);
        Task<bool> EditPost(EditPostViewModel model);
        Task<bool> RemovePost(string id);
        Task<List<Post>> GetAllPosts();
        Task<List<Post>> GetAuthorsPosts(string authorId);
        Task<PostsByAuthorViewModel> GetPostsByAuthor(string authorId);
        Task<PostViewModel> ViewPost(string id);
        Task<PostsByTagViewModel> GetPostsByTag(string id);
        Task<Post> GetPostById(string id);
    }
}
