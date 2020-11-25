using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.Entities;

namespace MovieShop.Core.RepositoryInterfaces
{
    public interface IMovieRepository : IAsyncRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetTopRatedMovie();
        Task<IEnumerable<Movie>> GetMovieByGenre(int genreId);
        Task<IEnumerable<Movie>> GetHighestRevenueMovies();
        Task<IEnumerable<Review>> GetReviewByMovie(int movieId);

    }
}