using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using MovieShop.Infrastructure.Data;
using MovieShop.Infrastructure.Repositories;
using MovieShop.Infrastructure.Services;


namespace MovieShop
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
            services.AddControllersWithViews();
            
            services.AddDbContext<MovieShopDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MovieShopDbConnection")));
            // .NET core has built-in IOC support
            // .NET framework doesn't have built-in IOC support, need 3rd party IOC like ninject, autofac
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IAsyncRepository<Favorite>, EfRepository<Favorite>>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IAsyncRepository<Genre>, EfRepository<Genre>>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<ICastRepository, CastRepository>();
            services.AddScoped<ICastService, CastService>();
            services.AddScoped<IAsyncRepository<Review>, EfRepository<Review>>();

            //sets the default authentication scheme for the app
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "MovieShopAuthCookie";
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                options.LoginPath = "/Account/Login";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            
            
            app.UseAuthentication(); //using cookie validate the login status
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