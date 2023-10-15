using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Comments
{
    public class AllCommentsViewModel
    {
        public string Info { get; set; }
        public List<Comment> Comments { get; set; }
    }
}

