using AutoMapper;
using BLL.Models.Comments;
using BLL.Models.Posts;
using BLL.Models.Roles;
using BLL.Models.Tags;
using BLL.Models.Users;
using DAL.Models;

namespace Blog
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreatePostViewModel, Post>();
            CreateMap<Post, CreatePostViewModel>();
            CreateMap<Post, EditPostViewModel>();
            CreateMap<Post, PostViewModel>();

            CreateMap<CreateCommentViewModel, Comment>()
                .ForMember(x => x.Author, opt => opt.MapFrom(m => m.AuthorId));
            CreateMap<Comment, CommentViewModel>()
                .ForMember(x => x.Post, opt => opt.Ignore());
            CreateMap<User, UserEditViewModel>();
            CreateMap<UserRegisterViewModel, User>();
            CreateMap<UserEditViewModel, User>();
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User > ();

            CreateMap<CreateTagViewModel, Tag>();
            CreateMap<TagViewModel, Tag>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.TagId));
            CreateMap<Tag, TagViewModel>()
                .ForMember(x => x.TagId, opt => opt.MapFrom(m => m.Id));
            CreateMap<Tag, EditTagViewModel>();
            CreateMap<EditTagViewModel, Tag>();
            CreateMap<Role, RoleViewModel>();
            CreateMap<CreateRoleViewModel, Role>();
            CreateMap<Role, CreateRoleViewModel>();
        }
    }
}
