using API.Data;
using API.DataAccess.Repository;
using API.DataAccess.Repository.IRepository;
using API.Mapper;
using API.Models.Models;
using API.Repository;
using API.Repository.IRepository;
using API.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // them
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IStoryRepository, StoryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ICategoryStoryRepository, CategoryStoryRepository>();
            services.AddScoped<IChapterRepository, ChapterRepository>();
            services.AddAutoMapper(typeof(Mappings));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("StoryOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Story API",
                    Version = "1",
                    Description = "Specialized projects",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "dotrongthuong8@gmail.com",
                        Name = "Do Trong Thuong",
                        Url = new Uri("https://github.com/trongthuong96"),
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/Mit_License")
                    }
                });
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(cmlCommentsFullPath);
            });

           /* services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();*/

            services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //them
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/StoryOpenAPISpec/swagger.json", "Story API");
                options.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //role
            app.UseAuthentication();
            CreateRoles(serviceProvider).Wait();
        }

        // Add roles
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { SD.Role_Admin, SD.Role_Maneger_User, SD.Role_User_Indi };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Here you could create a super user who will maintain the web app
            var poweruser = new ApplicationUser
            {

                UserName = Configuration.GetValue<string>("UserName"),
                Email = Configuration.GetValue<string>("UserEmail"),
                EmailConfirmed = true,
            };
            //Ensure you have these values in your appsettings.json file
            string userPWD = Configuration.GetValue<string>("UserPassword");
            var _user = await UserManager.FindByEmailAsync(Configuration.GetValue<string>("AdminUserEmail"));

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await UserManager.AddToRoleAsync(poweruser, SD.Role_Admin);

                }
            }
        }
    }
}
