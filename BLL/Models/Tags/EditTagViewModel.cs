﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Tags
{
    public class EditTagViewModel
    {
        [Required(ErrorMessage = "Укажите название тега")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Id { get; set; }
    }
}