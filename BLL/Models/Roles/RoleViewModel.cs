﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Roles
{
    public class RoleViewModel : CreateRoleViewModel
    {
        public string Id { get; set; }
        public bool IsChecked { get; set; }
        public List<User>? Users { get; set; }
    }
}
