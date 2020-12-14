using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MovieShop.Core.Entities;
using MovieShop.Core.Models;
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
                .Include(m => m.MovieCasts).ThenInclude(m => m.Cast)
                .Include(m => m.MovieGenres).ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return null;
            
            var movieRating = await _dbContext.Reviews.Where(r => r.MovieId == id).DefaultIfEmpty()
                 .AverageAsync(r => r == null ? 0 : r.Rating);
             if (movieRating > 0) movie.Rating = movieRating;
            return movie;
        }
        // public async Task<IEnumerable<Movie>> GetTopRatedMovie()
        // {
        //     var movieRank = await _dbContext.Reviews.Include(m => m.Movie)
        //         .GroupBy(r => new
        //         {
        //             Id = r.MovieId,
        //             r.Movie.Title,
        //             r.Movie.ReleaseDate
        //         })
        //         .OrderByDescending(g => g.Average(r => r.Rating))
        //         .Select(m => new Movie
        //         {
        //             Id = m.Key.Id,
        //             Title = m.Key.Title,
        //             ReleaseDate = m.Key.ReleaseDate,
        //             Rating = m.Average(r => r.Rating)
        //         }).Take(10).ToListAsync();
        //     return movieRank;
        // }

        public async Task<IEnumerable<Movie>> GetTopRatedMovie()
        {
            var topMovies = await _dbContext.Reviews.Include(r => r.Movie)
                .GroupBy(r => new
                {
                    Id = r.MovieId,
                    Title = r.Movie.Title,
                    PosterUrl = r.Movie.PosterUrl,
                }).OrderByDescending(g => g.Average(r => r.Rating))
                .Select(r => new Movie
                {
                    Id = r.Key.Id,
                    Title = r.Key.Title,
                    PosterUrl = r.Key.PosterUrl
                }).Take(5).ToListAsync();


            return topMovies;
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
            var reviews = await _dbContext.Reviews.Include(r=>r.Movie)
                .Include(r=>r.User)
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
            
            return reviews;
        }

        public async Task<IEnumerable<Movie>> GetHighestGrossingMovies()
        {
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(50).ToListAsync();
            //var movies = await _dbContext.Movies.Take(50).ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetTopPurchasedMovies()
        {
            var topPurchasedMovies = await _dbContext.Purchases.Include(p => p.Movie)
                .GroupBy(purchase => new
                {
                    Id = purchase.MovieId,
                    Title = purchase.Movie.Title,
                    PosterUrl = purchase.Movie.PosterUrl
                })
                .OrderByDescending(g => g.Count())
                .Select(g => new Movie
                {
                    Id = g.Key.Id,
                    Title = g.Key.Title,
                    PosterUrl = g.Key.PosterUrl
                }).Take(10).ToListAsync();
            return topPurchasedMovies;
        }
    }
}