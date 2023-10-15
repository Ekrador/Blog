using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Tags
{
    public class TagViewModel
    {
        public string TagId { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; }
    }
}
