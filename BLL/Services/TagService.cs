using AutoMapper;
using BLL.Extensions;
using BLL.Models.Posts;
using BLL.Models.Tags;
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
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagRep;
        private readonly IMapper _mapper;
        public TagService(IRepository<Tag> tagRep, IMapper mapper)
        {
            _tagRep = tagRep;
            _mapper = mapper;
        }

        public async Task<bool> CreateTag(CreateTagViewModel model)
        {
            var tag = _mapper.Map<Tag>(model);
            return await _tagRep.Create(tag);
        }

        public async Task<EditTagViewModel> EditTag(string id)
        {
            EditTagViewModel tagModel = null;
            var tag = await _tagRep.Get(id);
            if (tag != null)
            {
                tagModel = _mapper.Map<EditTagViewModel>(tag);
            }
            return tagModel;
        }

        public async Task<bool> EditTag(EditTagViewModel model)
        {
            var tag = await _tagRep.Get(model.Id);
            tag.Name = model.Name;
            return await _tagRep.Update(tag);
        }

        public async Task<List<Tag>> GetAllTags()
        {
            var tags = await _tagRep.GetAll();
            if (tags != null)
            {
                foreach(var tag in tags)
                await _tagRep.LoadAllNavigationPropertiesAsync(tag);
            }
            return tags.ToList();
        }

        public async Task<Tag> GetTag(string id)
        {
            var tag = await _tagRep.Get(id);
            return tag;
        }

        public async Task<bool> RemoveTag(string id)
        {
            var tag = await _tagRep.Get(id);
            return await _tagRep.Delete(tag);
        }
    }
}
