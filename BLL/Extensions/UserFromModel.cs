using BLL.Models.Users;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class UserFromModel
    {
        public static User Convert(this User user, UserEditViewModel usereditvm)
        {
            user.Avatar = usereditvm.Avatar;
            user.LastName = usereditvm.LastName;
            user.MiddleName = usereditvm.MiddleName;
            user.FirstName = usereditvm.FirstName;
            user.Email = usereditvm.Email;
            user.BirthDate = usereditvm.BirthDate;
            user.UserName = usereditvm.UserName;
            user.About = usereditvm.About;
            user.RegistrationDate = usereditvm.RegistrationDate;
            user.Posts = usereditvm.Posts;
            user.Comments = usereditvm.Comments;     
            
            return user;
        }
    }
}
