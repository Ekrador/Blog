using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Roles
{
    public static class StandartRoles
    {
        public static List<string> Roles { get; } = new List<string> {"Администратор", "Пользователь", "Модератор" };
    }
}
