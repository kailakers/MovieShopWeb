using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.Entities;
using MovieShop.Core.Helpers;
using MovieShop.Core.Models;
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
        public MovieService(IMovieRepository movieRepository, IAsyncRepository<Favorite> favoriteRepository)
        {
            _movieRepository = movieRepository;
            _favoriteRepository = favoriteRepository;
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
                Rating = movie.Rating,
                Overview = movie.Overview,
                Tagline = movie.Tagline,
                Budget = movie.Budget,
                Revenue = movie.Revenue,
                ImdbUrl = movie.ImdbUrl,
                TmdbUrl = movie.TmdbUrl,
                ReleaseDate = movie.ReleaseDate,
                RunTime = movie.RunTime,
                Price = movie.Price,
                FavoritesCount = favoriteCount
            };
            
            return movieDetailsResponseModel;
        }

        public async Task<IEnumerable<ReviewMovieResponseModel>> GetReviewsForMovie(int id)
        {
            var reviews = await _movieRepository.GetReviewByMovie(id);
            var reviewMovieResponseModel = new List<ReviewMovieResponseModel>();
            foreach (var review in reviews)
            {
                reviewMovieResponseModel.Add(new ReviewMovieResponseModel
                {
                    MovieId = review.MovieId, Name = review.User.FirstName, Rating = review.Movie.Rating,
                    ReviewText = review.ReviewText, UserId = review.UserId
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
                    Id = movie.Id, PosterUrl = movie.PosterUrl, Title = movie.Title, ReleaseDate = movie.ReleaseDate.Value
                });
            }

            return movieResponseModel;
        }

        public Task<IEnumerable<MovieResponseModel>> GetHighestGrossingMovies()
        {
            throw new System.NotImplementedException();
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

        public Task<MovieDetailsResponseModel> CreateMovie(MovieCreateRequest movieCreateRequest)
        {
            throw new System.NotImplementedException();
        }

        public Task<MovieDetailsResponseModel> UpdateMovie(MovieCreateRequest movieCreateRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}