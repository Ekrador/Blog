using BLL.Models.Post;
using BLL.Models.Tags;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.IServices
{
    public interface ITagService
    {
        Task<bool> CreateTag(CreateTagViewModel model);

        Task<bool> EditTag(EditTagViewModel model);

        Task<bool> RemoveTag(string id);

        Task<List<Tag>> GetAllTags();

        Task<Tag> GetTag(string id);
    }
}
