using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;

namespace MovieShop.Infrastructure.Repositories
{
    public class MovieRepository: EfRepository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<Movie>> GetTopRatedMovie()
        {
            var MovieRank = await _dbContext.Reviews.Include(m => m.Movie)
                .GroupBy(r => new
                {
                    Id = r.MovieId,
                    r.Movie.Title,
                    r.Movie.ReleaseDate
                })
                .OrderByDescending(g => g.Average(r => r.Rating))
                .Select(m => new Movie
                {
                    Id = m.Key.Id,
                    Title = m.Key.Title,
                    ReleaseDate = m.Key.ReleaseDate,
                    Rating = m.Average(r => r.Rating)
                }).Take(10).ToListAsync();
            return MovieRank;
        }

        public async Task<IEnumerable<Movie>> GetMovieByGenre(int genreId)
        {
            return await _dbContext.MovieGenres.Include(m => m.Movie)
                .Where(g => g.GenreId == genreId)
                .Select(mg => mg.Movie).ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetHighestRevenueMovies()
        {
            return await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(1).ToListAsync();
        }
    }
}