using MovieShop.Core.Entities;
using MovieShop.Core.Models.Response;

namespace MovieShop.Core.ServiceInterfaces
{
    public interface IJwtService
    {
        string GenerateToken(UserLoginResponseModel user);
    }
}