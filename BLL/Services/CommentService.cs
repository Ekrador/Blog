using AutoMapper;
using BLL.Models.Comments;
using BLL.Services.IServices;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentRep;
        private readonly IMapper _mapper;
        public CommentService(IRepository<Comment> commentRep, IMapper mapper)
        {
            _commentRep = commentRep;
            _mapper = mapper;
        }

        public async Task<bool> EditComment(EditCommentViewModel model)
        {
            var comment = _mapper.Map<Comment>(model);
            return await _commentRep.Update(comment);
        }

        public async Task<List<Comment>> GetAllComments()
        {
            var comments = await _commentRep.GetAll();
            return comments.ToList();
        }

        public async Task<Comment> GetComment(string id)
        {
            var comment = await _commentRep.Get(id);
            return comment;
        }

        public async Task<bool> RemoveComment(string id)
        {
            var post = await _commentRep.Get(id);
            return await _commentRep.Delete(post);
        }

        public async Task<bool> WriteComment(CreateCommentViewModel model)
        {
            var comment = _mapper.Map<Comment>(model);
            return await _commentRep.Create(comment);
        }
    }
}
