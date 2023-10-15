using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.News
{
    public class EditNewsViewModel : AddNewsViewModel
    {
        public string Id { get; set; }
    }
}
