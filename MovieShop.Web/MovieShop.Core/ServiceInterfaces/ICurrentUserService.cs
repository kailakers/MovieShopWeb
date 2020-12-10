using System.Collections.Generic;
using System.Security.Claims;

namespace MovieShop.Core.ServiceInterfaces
{
    public interface ICurrentUserService
    {
        // access modifier default is internal
        int? UserId { get; set; }
        bool IsAuthenticated { get; set; }
        string UserName { get; set; }
        string FullName { get; set; }
        string Email { get; set; }
        string RemoteIpAddress { get; set; }
        IEnumerable<Claim> GetClaimsIdentity();
        IEnumerable<string> Roles { get; }
    }
}