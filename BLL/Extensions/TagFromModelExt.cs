using BLL.Models.Tags;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class TagFromModelExt
    {
        public static Tag Convert(this Tag tag, EditTagViewModel tageditvm)
        {
            tag.Name = tageditvm.Name;
            return tag;
        }
    }
}
