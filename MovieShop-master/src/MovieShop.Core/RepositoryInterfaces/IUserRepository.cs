using System.Threading.Tasks;
using MovieShop.Core.Entities;

namespace MovieShop.Core.RepositoryInterfaces
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User> GetUserByEmail(string email);
    }
}