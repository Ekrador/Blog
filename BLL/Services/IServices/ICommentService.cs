using BLL.Models.Comments;
using BLL.Models.Post;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.IServices
{
    public interface ICommentService
    {
        Task<bool> WriteComment(CreateCommentViewModel model);

        Task<bool> EditComment(EditCommentViewModel model);

        Task<bool> RemoveComment(string id);

        Task<List<Comment>> GetAllComments();

        Task<Comment> GetComment(string id);
    }
}
