using BLL.Contracts.Responses;
using BLL.Models.Posts;
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
        Task<EditTagViewModel> EditTag(string id);

        Task<bool> RemoveTag(string id);

        Task<List<Tag>> GetAllTags();
        Task<AllTagsResponse> GetAllTagsResponse();

        Task<Tag> GetTag(string id);
        Task<TagViewResponse> GetTagResponse(string id);
    }
}
