using DAL.Models;

namespace BLL.Contracts.Responses
{
    public class AllUsersResponse
    {
        public int UsersAmount { get; set; }
        public List<UserViewResponse> Users { get; set; }  
    }
    public class UserViewResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? MiddleName { get; set; }
        public string About { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<string> Roles { get; set; }
        public int PostsCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
