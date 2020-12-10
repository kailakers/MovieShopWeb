using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieShop.Core.Entities;
using MovieShop.Core.Helpers;
using MovieShop.Core.Models;
using MovieShop.Core.Models.Response;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using MovieShop.Infrastructure.Data;
using MovieShop.Infrastructure.Repositories;

namespace MovieShop.Infrastructure.Services
{
    public class MovieService: IMovieService
    {
        // DI pattern allows us to write the lossly coupling code which is more maintainable and testable
        private readonly IMovieRepository _movieRepository;
        private readonly IAsyncRepository<Favorite> _favoriteRepository;
        private readonly ICurrentUserService _currentUserService;
        public MovieService(IMovieRepository movieRepository, IAsyncRepository<Favorite> favoriteRepository, 
            ICurrentUserService currentUserService)
        {
            _movieRepository = movieRepository;
            _favoriteRepository = favoriteRepository;
            _currentUserService = currentUserService;
        }

        public Task<PagedResultSet<MovieResponseModel>> GetMoviesByPagination(int pageSize = 20, int page = 0, string title = "")
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedResultSet<MovieResponseModel>> GetAllMoviePurchasesByPagination(int pageSize = 20, int page = 0)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<MovieResponseModel>> GetAllPurchasesByMovieId(int movieId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<MovieDetailsResponseModel> GetMovieAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            var favoriteCount = await _favoriteRepository.GetCountAsync(f => f.Id == id);
            MovieDetailsResponseModel movieDetailsResponseModel = new MovieDetailsResponseModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                PosterUrl = movie.PosterUrl,
                BackdropUrl = movie.BackdropUrl,
                Overview = movie.Overview,
                Tagline = movie.Tagline,
                Budget = movie.Budget,
                Revenue = movie.Revenue,
                ImdbUrl = movie.ImdbUrl,
                TmdbUrl = movie.TmdbUrl,
                ReleaseDate = movie.ReleaseDate,
                RunTime = movie.RunTime,
                Price = movie.Price,
                FavoritesCount = favoriteCount,
                Casts = new List<MovieDetailsResponseModel.CastResponseModel>(),
                Genres = new List<Genre>()
            };
            foreach (var cast in movie.MovieCasts)
            {
                movieDetailsResponseModel.Casts.Add(new MovieDetailsResponseModel.CastResponseModel
                {
                    Id = cast.Cast.Id,
                    Character = cast.Character,
                    Gender = cast.Cast.Gender,
                    Name = cast.Cast.Name,
                    TmdbUrl = cast.Cast.TmdbUrl,
                    ProfilePath = cast.Cast.ProfilePath
                });
            }

            foreach (var genre in movie.MovieGenres)
            {
                movieDetailsResponseModel.Genres.Add(genre.Genre);
            }
            
            return movieDetailsResponseModel;
        }

        public async Task<IEnumerable<ReviewMovieResponseModel>> GetReviewsForMovie(int id)
        {
            var reviews = await _movieRepository.GetReviewByMovie(id);
            
            List<ReviewMovieResponseModel> reviewMovieResponseModel = new List<ReviewMovieResponseModel>();
            foreach (var review in reviews)
            {
                reviewMovieResponseModel.Add(new ReviewMovieResponseModel
                {
                    MovieId = review.MovieId,
                    Name = review.User.FirstName, 
                    Rating = review.Rating,
                    ReviewText = review.ReviewText, 
                    UserId = review.UserId
                });
            }

            return reviewMovieResponseModel;
        }

        public async Task<int> GetMoviesCount(string title = "")
        {
            var movieCount = await _movieRepository.GetCountAsync(m => m.Title == title);
            return movieCount;
        }

        public async Task<IEnumerable<MovieResponseModel>> GetTopRatedMovies()
        {
            var movies = await _movieRepository.GetTopRatedMovie();
            var movieResponseModel = new List<MovieResponseModel>();
            foreach (var movie in movies)
            {
                movieResponseModel.Add(new MovieResponseModel
                {
                    Id = movie.Id, PosterUrl = movie.PosterUrl, Title = movie.Title, ReleaseDate = movie.ReleaseDate
                });
            }

            return movieResponseModel;
        }

        public async Task<IEnumerable<MovieResponseModel>> GetHighestGrossingMovies()
        {
            var movies = await _movieRepository.GetHighestGrossingMovies();
            var movieResponseModel = new List<MovieResponseModel>();
            foreach (var movie in movies)
            {
                movieResponseModel.Add(new MovieResponseModel
                {
                    Id = movie.Id, PosterUrl = movie.PosterUrl, Title = movie.Title, ReleaseDate = movie.ReleaseDate
                });
            }

            return movieResponseModel;
        }

        public async Task<IEnumerable<MovieResponseModel>> GetMoviesByGenre(int genreId)
        {
            var movies = await _movieRepository.GetMovieByGenre(genreId);
            var movieResponseModel = new List<MovieResponseModel>();
            foreach (var movie in movies)
            {
                movieResponseModel.Add(new MovieResponseModel
                {
                    Id = movie.Id, Title = movie.Title, PosterUrl = movie.PosterUrl, ReleaseDate = movie.ReleaseDate.Value
                });
            }
            return movieResponseModel;
        }

        public async Task<MovieDetailsResponseModel> CreateMovie(MovieCreateRequest movieCreateRequest)
        {
            var roles = _currentUserService.Roles;
            List<string> rolesList = new List<string>();
            foreach (var role in roles)
            {
                rolesList.Add(role);
            }

            string admin = "Admin";
            if (!rolesList.Exists(e=>e==admin))
                throw new Exception("Not Authorized");
            var movie = new Movie
            {
                Title = movieCreateRequest.Title,
                Overview = movieCreateRequest.Overview,
                BackdropUrl = movieCreateRequest.BackdropUrl,
                Budget = movieCreateRequest.Budget,
                CreatedDate = movieCreateRequest.CreatedDate,
                ImdbUrl = movieCreateRequest.ImdbUrl,
                OriginalLanguage = movieCreateRequest.OriginalLanguage,
                PosterUrl = movieCreateRequest.PosterUrl,
                Price = movieCreateRequest.Price,
                ReleaseDate = movieCreateRequest.ReleaseDate,
                RunTime = movieCreateRequest.RunTime,
                Tagline = movieCreateRequest.TagLine,
                TmdbUrl = movieCreateRequest.TmdbUrl
            };
            var createdMovie = await _movieRepository.AddAsync(movie);
            var response = new MovieDetailsResponseModel()
            {
                Id = createdMovie.Id, 
                Title = createdMovie.Title,
                Overview = createdMovie.Overview,
                BackdropUrl = createdMovie.BackdropUrl,
                Budget = createdMovie.Budget,
                CreatedDate = createdMovie.CreatedDate,
                ImdbUrl = createdMovie.ImdbUrl,
                OriginalLanguage = createdMovie.OriginalLanguage,
                PosterUrl = createdMovie.PosterUrl,
                Price = createdMovie.Price,
                ReleaseDate = createdMovie.ReleaseDate,
                RunTime = createdMovie.RunTime,
                Tagline = createdMovie.Tagline,
                TmdbUrl = createdMovie.TmdbUrl
            };
            return response;
        }

        public Task<MovieDetailsResponseModel> UpdateMovie(MovieCreateRequest movieCreateRequest)
        {
            var roles = _currentUserService.Roles;
            List<string> rolesList = new List<string>();
            foreach (var role in roles)
            {
                rolesList.Add(role);
            }

            string admin = "Admin";
            if (!rolesList.Exists(e=>e==admin))
                throw new Exception("Not Authorized");
            
            var movie = new Movie
            {
                Title = movieCreateRequest.Title,
                Overview = movieCreateRequest.Overview,
                BackdropUrl = movieCreateRequest.BackdropUrl,
                Budget = movieCreateRequest.Budget,
                CreatedDate = movieCreateRequest.CreatedDate,
                ImdbUrl = movieCreateRequest.ImdbUrl,
                OriginalLanguage = movieCreateRequest.OriginalLanguage,
                PosterUrl = movieCreateRequest.PosterUrl,
                Price = movieCreateRequest.Price,
                ReleaseDate = movieCreateRequest.ReleaseDate,
                RunTime = movieCreateRequest.RunTime,
                Tagline = movieCreateRequest.TagLine,
                TmdbUrl = movieCreateRequest.TmdbUrl
            };
            var createMovie = _movieRepository.UpdateAsync(movie);
            //var response = new MovieD
            return null;
        }

        public async Task<IEnumerable<MovieResponseModel>> GetTopPurchasedMovie()
        {
            var roles = _currentUserService.Roles;
            List<string> rolesList = new List<string>();
            foreach (var role in roles)
            {
                rolesList.Add(role);
            }

            string admin = "Admin";
            if (!rolesList.Exists(e=>e==admin))
                throw new Exception("Not Authorized");
            
            var movies = await _movieRepository.GetTopPurchasedMovies();
            List<MovieResponseModel> movieResponseModel = new List<MovieResponseModel>();
            foreach (var movie in movies)
            {
                movieResponseModel.Add(new MovieResponseModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterUrl = movie.PosterUrl
                });
            }

            return movieResponseModel;
        }

        public async Task<IEnumerable<MovieResponseModel>> GetAllMovies()
        {
            var movies = await _movieRepository.ListAllAsync();
            List<MovieResponseModel> movieResponseModels = new List<MovieResponseModel>();
            foreach (var movie in movies)
            {
                movieResponseModels.Add(new MovieResponseModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterUrl = movie.PosterUrl,
                    ReleaseDate = movie.ReleaseDate
                });
            }

            return movieResponseModels;
        }
    }
}