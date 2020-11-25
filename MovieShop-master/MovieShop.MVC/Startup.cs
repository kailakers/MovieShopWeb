using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieShop.Core.MappingProfiles;
using MovieShop.Infrastructure.Data;
using MovieShop.Infrastructure.Helpers;
using MovieShop.MVC.Infrastructure;

namespace MovieShop.MVC
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
            services.AddDbContext<MovieShopDbContext>(options =>
                options.UseSqlServer(Configuration
                    .GetConnectionString("MovieShopDbConnection")));
            services.AddControllersWithViews();

            services.AddAutoMapper(typeof(Startup), typeof(MoviesMappingProfile));

            //sets the default authentication scheme for the app
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
                {
                    options.Cookie.Name = "MovieShopAuthCookie";
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.LoginPath = "/Account/Login";
                });
            ConfigureDependencyInjection(services);
            services.AddMemoryCache();
            services.AddHttpContextAccessor();

        }

        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddRepositories();
            services.AddServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
              //  app.UseDeveloperExceptionPage();
               // app.UseExceptionHandler("/Home/Error");
               app.UseMovieShopExceptionMiddleware();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
