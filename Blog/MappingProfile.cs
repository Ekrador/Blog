using AutoMapper;
using BLL.Models.Comments;
using BLL.Models.Post;
using BLL.Models.Users;
using DAL.Models;

namespace Blog
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreatePostViewModel, Post>()
                .ForMember(x => x.Author, opt => opt.MapFrom(m => m.AuthorId));
            CreateMap<CreateCommentViewModel, Comment>()
                .ForMember(x => x.Author, opt => opt.MapFrom(m => m.AuthorId));
            CreateMap<User, UserEditViewModel>();
            CreateMap<UserRegisterViewModel, User>();
            CreateMap<UserEditViewModel, User>();
        }
    }
}
