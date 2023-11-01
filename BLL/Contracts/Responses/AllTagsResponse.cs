using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts.Responses
{
    public class AllTagsResponse
    {
        public int TagsAmount { get; set; }
        public List<TagViewResponse> Tags { get; set; }
    }
    public class TagViewResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> PostsIds { get; set; } 
    }
}
