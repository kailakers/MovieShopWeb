using System.Threading.Tasks;
using MovieShop.Core.Models.Response;

namespace MovieShop.Core.ServiceInterfaces
{
    public interface ICastService
    {
        Task<CastDetailsResponseModel> GetCastDetailsWithMovies(int id);
    }
}