using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.Entities;

namespace MovieShop.Core.ServiceInterfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllGenres();
    }
}