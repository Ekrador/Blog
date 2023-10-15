using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Comments
{
    public class CommentsByAuthorViewModel :AllCommentsViewModel
    {
        public string AuthorName { get; set; }
    }
}
