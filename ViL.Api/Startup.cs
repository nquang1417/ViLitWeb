using Microsoft.EntityFrameworkCore;
using ViL.Data;
using ViL.Data.Repositories;
using ViL.Services.Services;

namespace ViL.Api
{
    public class Startup
    {
        public IConfiguration _configuration;

        public Startup(IConfiguration configuration) { _configuration = configuration; }
        public void ConfigureServices(IServiceCollection services)
        {
            var ViLDb = "";
            if (_configuration != null)
            {
                ViLDb = _configuration.GetConnectionString("ViLitDb");
            }
            services.AddDbContext<ViLDbContext>(options =>
            {
                options.UseSqlServer(ViLDb);
            });
            services.AddControllersWithViews();
            services.AddCors();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            /// Add Services
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

            services.AddScoped<IUsersRepository,UsersRepository>();
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
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run();
        }
    }
}
