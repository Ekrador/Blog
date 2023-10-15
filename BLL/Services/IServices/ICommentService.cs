using BLL.Models.Comments;
using BLL.Models.Posts;
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
        Task<EditCommentViewModel> EditComment(string id);
        Task<bool> RemoveComment(string id);
        Task<List<Comment>> GetAllComments();
        Task<CommentsByAuthorViewModel> GetCommentsByAuthor(string authorId);
        Task<CommentViewModel> ViewComment(string id);
    }
}
