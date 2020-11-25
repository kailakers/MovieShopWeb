using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.Models.Response;

namespace MovieShop.Core.RepositoryInterfaces
{
    public interface IChartRepository
    {
        Task<IEnumerable<MovieChartResponseModel>> GetTopPurchasedMovies();
    }
}