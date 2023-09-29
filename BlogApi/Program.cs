using AutoMapper;
using BLL.Services.IServices;
using BLL.Services;
using DAL.Context;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Blog;

namespace BlogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            // Add services to the container.

            var mapperConfig = new MapperConfiguration((v) =>
            {
                v.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            builder.Services.AddDbContext<BlogContext>(options => options.UseSqlite(connection, builder =>
            {
                builder.MigrationsAssembly("Blog");
            }))
                .AddIdentity<User, Role>(opts => {
                    opts.Password.RequiredLength = 5;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                    opts.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<BlogContext>();
            builder.Services.AddSingleton(mapper)
                .AddTransient<IRepository<Comment>, CommentRepository>()
                .AddTransient<IRepository<Post>, PostRepository>()
                .AddTransient<IRepository<Tag>, TagRepository>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<ICommentService, CommentService>()
                .AddTransient<IPostService, PostService>()
                .AddTransient<ITagService, TagService>()
                .AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}