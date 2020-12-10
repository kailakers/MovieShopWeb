using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.Entities;
using MovieShop.Core.Models;

namespace MovieShop.Core.RepositoryInterfaces
{
    public interface ICastRepository
    {
        Task<Cast> GetCastDetailById(int id);
    }
}