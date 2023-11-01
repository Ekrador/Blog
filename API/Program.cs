using AutoMapper;
using BLL.Services.IServices;
using BLL.Services;
using DAL.Context;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Blog;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace API
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
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog_API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");

            var mapperConfig = new MapperConfiguration((v) =>
            {
                v.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            builder.Services.AddDbContext<BlogContext>(options => options.UseSqlite(connection, builder =>
            {
                builder.MigrationsAssembly("Blog");
            }))
                .AddIdentity<User, Role>(opts =>
                {
                    opts.Password.RequiredLength = 5;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                    opts.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<BlogContext>();

            builder.Services.AddSingleton(mapper);

            builder.Services
                .AddTransient<IRepository<Comment>, CommentRepository>()
                .AddTransient<IRepository<Post>, PostRepository>()
                .AddTransient<IRepository<Tag>, TagRepository>()
                .AddTransient<IRepository<News>, NewsRepository>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<ICommentService, CommentService>()
                .AddTransient<IPostService, PostService>()
                .AddTransient<IRoleService, RoleService>()
                .AddTransient<ITagService, TagService>()
                .AddTransient<INewsService, NewsService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/user/login";
                    options.AccessDeniedPath = "/accessdenied";
                });
            builder.Services.AddAuthorization();

            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
            });

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog();
                NLog.LogManager.Setup().LoadConfiguration(builder => builder.SetTimeSource(new NLog.Time.AccurateUtcTimeSource()));
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Map("account/login", () => Results.Problem(
                type: "/docs/errors/unauthorized",
                title: "Необходима авторизация",
                statusCode: StatusCodes.Status401Unauthorized
                ));

            app.Map("account/accessdenied", () => Results.Problem(
                type: "/docs/errors/forbidden",
                title: "Доступ запрещен",
                statusCode: StatusCodes.Status403Forbidden
                ));

            app.Run();
        }
    }
}