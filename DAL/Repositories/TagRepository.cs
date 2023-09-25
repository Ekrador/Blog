using DAL.Context;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(BlogContext db) : base(db)
        {
        }
    }
}
