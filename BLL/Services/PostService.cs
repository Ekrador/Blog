using AutoMapper;
using BLL.Models.Post;
using BLL.Services.IServices;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    internal class PostService : IPostService
    {
        private readonly IRepository<Post> _postRep;
        private readonly IMapper _mapper;
        public PostService(IRepository<Post> repository, IMapper mapper)
        {
            _postRep = repository;
            _mapper = mapper;
        }
        public async Task<bool> CreatePost(CreatePostViewModel model)
        {
            var post = _mapper.Map<Post>(model);
            return await _postRep.Create(post);
        }

        public async Task<bool> EditPost(EditPostViewModel model)
        {
            var post = _mapper.Map<Post>(model);
            return await _postRep.Update(post);
        }

        public async Task<List<Post>> GetAllPosts()
        {
            var posts = await _postRep.GetAll();
            return posts.ToList();
        }

        public async Task<List<Post>> GetAuthorsPosts(string authorId)
        {
            var posts = await _postRep.GetAll();
            var authorPosts = posts.Where(p => p.Author.Id == authorId);
            return authorPosts.ToList();
        }

        public async Task<bool> RemovePost(string id)
        {
            var post = await _postRep.Get(id);
            return await _postRep.Delete(post);
        }
    }
}
