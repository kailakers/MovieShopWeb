using Microsoft.Extensions.DependencyInjection;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using MovieShop.Infrastructure.Repositories;
using MovieShop.Infrastructure.Services;

namespace MovieShop.Infrastructure.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<ICastRepository, CastRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAsyncRepository<Favorite>, EfRepository<Favorite>>();
            services.AddScoped<IAsyncRepository<Purchase>, EfRepository<Purchase>>();
            services.AddScoped<IAsyncRepository<Genre>, EfRepository<Genre>>();
            services.AddScoped<IAsyncRepository<Review>, EfRepository<Review>>();
            services.AddScoped<IAsyncRepository<MovieGenre>, EfRepository<MovieGenre>>();
            services.AddScoped<IAsyncRepository<UserRole>, EfRepository<UserRole>>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IChartRepository, ChartRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<ICastService, CastService>();
            services.AddScoped<IJwtService, JwtService>();
        }
    }
}