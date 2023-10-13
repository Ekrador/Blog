using BLL.Models.Comments;
using BLL.Models.Posts;
using BLL.Models.Users;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class UserFromModelExt
    {
        public static User Convert(this User user, UserEditViewModel usereditvm)
        {
            user.Avatar = usereditvm.Avatar ?? user.Avatar;
            user.LastName = usereditvm.LastName ?? user.LastName;
            user.MiddleName = usereditvm.MiddleName ?? user.MiddleName;
            user.FirstName = usereditvm.FirstName ?? user.FirstName;
            user.Email = usereditvm.Email ?? user.Email;
            user.BirthDate = usereditvm.BirthDate == new DateTime() ? user.BirthDate : usereditvm.BirthDate;
            user.UserName = usereditvm.UserName ?? user.UserName;
            user.About = usereditvm.About ?? user.About;            
            return user;
        }
    }
}
