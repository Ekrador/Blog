using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Posts
{
    public class CreatePostApiModel: CreatePostViewModel
    {
        new public List<string>? Tags { get; set; }
    }
}
