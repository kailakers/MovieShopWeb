using System.Threading.Tasks;
using MovieShop.Core.Models;

namespace MovieShop.Core.ServiceInterfaces
{
    public interface ICastService
    {
        Task<CastDetailsResponseModel> GetCastDetailsById(int id);
    }
}