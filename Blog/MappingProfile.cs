using AutoMapper;
using BLL.Contracts.Responses;
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
            CreateMap<CreatePostApiModel, CreatePostViewModel>()
                .ForMember(x=> x.Tags, opt => opt.Ignore());
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
            CreateMap<UserEditApiModel, UserEditViewModel>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .AfterMap((src, dest) => ReplaceApiStringsWithNull(src, dest));
            CreateMap<User, UserViewResponse>()
                .ForMember(x => x.PostsCount, opt => opt.MapFrom(m => m.Posts.Count()))
                .ForMember(x => x.CommentsCount, opt => opt.MapFrom(m => m.Comments.Count()));
            CreateMap<CommentViewModel, CommentViewResponse>()
                .ForMember(x => x.PostId, opt => opt.MapFrom(m => m.Post.Id))
                .ForMember(x => x.AuthorId, opt => opt.MapFrom(m => m.Author.Id));
            CreateMap<News, NewsViewResponse>();
            CreateMap<Post, PostViewResponse>()
                .ForMember(x => x.AuthorId, opt => opt.MapFrom(m => m.Author.Id))
                .ForMember(x => x.Tags, opt => opt.MapFrom(m => m.Tags.Select(t => t.Name).ToList()))
                .ForMember(x => x.CommentsIds, opt => opt.MapFrom(m => m.Comments.Select(c => c.Id).ToList()));
            CreateMap<EditPostApiModel, EditPostViewModel>()
                .ForMember(x => x.Tags, opt => opt.Ignore())
                .AfterMap((src, dest) => ReplaceApiStringsWithNull(src, dest));
            CreateMap<Role, RoleViewResponse>()
                .ForMember(x => x.UsersIds, opt => opt.Ignore());
            CreateMap<Tag, TagViewResponse>()
                .ForMember(x => x.PostsIds, opt => opt.MapFrom(m => m.Posts.Select(p => p.Id).ToList()));
        }

        public void ReplaceApiStringsWithNull<T1, T2>(T1 source, T2 destination)
        {
            foreach (var property in typeof(T2).GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string)property.GetValue(destination, null);
                    if (value == "string")
                    {
                        property.SetValue(destination, null, null);
                    }
                }
            }
        }
    }
}
