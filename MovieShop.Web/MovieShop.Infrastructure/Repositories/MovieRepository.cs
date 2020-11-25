using System;
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
        public override async Task<Movie> GetByIdAsync(int id)
        {
            var movie = await _dbContext.Movies
                .Include(m => m.MovieCasts).ThenInclude(m => m.Cast).Include(m => m.MovieGenres)
                .ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return null;
            var movieRating = await _dbContext.Reviews.Where(r => r.MovieId == id).DefaultIfEmpty()
                .AverageAsync(r => r == null ? 0 : r.Rating);
            if (movieRating > 0) movie.Rating = movieRating;
            return movie;
        }
        public async Task<IEnumerable<Movie>> GetTopRatedMovie()
        {
            var movieRank = await _dbContext.Reviews.Include(m => m.Movie)
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
            return movieRank;
        }

        public async Task<IEnumerable<Movie>> GetMovieByGenre(int genreId)
        {
            return await _dbContext.MovieGenres.Include(m => m.Movie)
                .Where(g => g.GenreId == genreId)
                .Select(mg => mg.Movie).ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetHighestRevenueMovies()
        {
            return await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(50).ToListAsync();
            // skip and take in linq ==> offset and fetch in sql
        }

        public async Task<IEnumerable<Review>> GetReviewByMovie(int movieId)
        {
            return await _dbContext.Reviews.Include(r=>r.Movie)
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
        }
    }
}