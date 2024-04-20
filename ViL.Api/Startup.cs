using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using ViL.Api.Models;
using ViL.Data;
using ViL.Data.Infrastructure;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;
using ViL.Services.Services;

namespace ViL.Api
{
    public class Startup
    {
        public IConfiguration _configuration;

        public Startup(IConfiguration configuration) { _configuration = configuration; }
        public void ConfigureServices(WebApplicationBuilder builder ,IServiceCollection services)
        {
            var ViLDb = "";
            var HangfireDb = "";
            if (_configuration != null)
            {
                ViLDb = _configuration.GetConnectionString("ViLitDb");
                HangfireDb = _configuration.GetConnectionString("HangfireDb");
            }
            
            services.AddDbContext<ViLDbContext>(options =>
            {
                options.UseSqlServer(ViLDb);
            });
            services.AddControllersWithViews();
            /*services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });*/
            /*services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Admin");
                });
            });*/
            services.AddHangfire(x =>
            {
                x.UseSqlServerStorage(HangfireDb);
            });
            services.AddHangfireServer();
            services.AddCors();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            /// Add Services
            //services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
            //services.AddScoped(typeof(IServices<>), typeof(ServiceBase<>));

            services.AddScoped<IBookChaptersRepository, BookChaptersRepository>();
            services.AddScoped<IBookChaptersService, BookChaptersService>();

            services.AddScoped<IBookInfoRepository, BookInfoRepository>();
            services.AddScoped<IBookInfoService, BookInfoService>();

            services.AddScoped<IBookmarksRepository, BookmarksRepository>();
            services.AddScoped<IBookmarksService, BookmarksService>();

            services.AddScoped<IBookReviewsRepository, BookReviewsRepository>();
            services.AddScoped<IBookReviewsService, BookReviewsService>();

            services.AddScoped<IBookStatisticsInfoRepository, BookStatisticsInfoRepository>();
            services.AddScoped<IBookStatisticsInfoService, BookStatisticsInfoService>();

            services.AddScoped<IBookTagsRepository, BookTagsRepository>();
            services.AddScoped<IBookTagsService, BookTagsService>();

            services.AddScoped<IChapterCommentsRepository, ChapterCommentsRepository>();
            services.AddScoped<IChapterCommentsService, ChapterCommentsService>();

            services.AddScoped<IGenresRepository, GenresRepository>();
            services.AddScoped<IGenresService, GenresService>();

            services.AddScoped<INotificationsRepository, NotificationsRepository>();
            services.AddScoped<INotificationsService, NotificationsService>();

            services.AddScoped<IReadingHistoryRepository, ReadingHistoryRepository>();
            services.AddScoped<IReadingHistoryService, ReadingHistoryService>();

            services.AddScoped<IReportsRepository, ReportsRepository>();
            services.AddScoped<IReportsService, ReportsService>();

            services.AddScoped<ITagsRepository, TagsRepository>();
            services.AddScoped<ITagsService, TagsService>();

            services.AddScoped<IUserFavoriteBooksRepository, UserFavoriteBooksRepository>();
            services.AddScoped<IUserFavoriteBooksService, UserFavoriteBooksService>();

            services.AddScoped<IUserLikedChaptersRepository, UserLikedChaptersRepository>();
            services.AddScoped<IUserLikedChaptersService, UserLikedChaptersService>();

            services.AddScoped<IUserLikedCommentsRepository, UserLikedCommentsRepository>();
            services.AddScoped<IUserLikedCommentsService, UserLikedCommentsService>();

            services.AddScoped<IUserLikedReviewsRepository, UserLikedReviewsRepository>();
            services.AddScoped<IUserLikedReviewsService, UserLikedReviewsService>();

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUsersService, UsersService>();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            } else
            {

            }
            app.UseHangfireDashboard();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run();
        }

        
    }
}
