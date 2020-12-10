using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MovieShop.Core.ServiceInterfaces;
using Microsoft.AspNetCore.Http;

namespace MovieShop.Infrastructure.Services
{
    public class CurrentUserService:ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Name => GetName();

        public int? UserId
        {
            get => GetUserId();
            set => throw new NotImplementedException();
        }

        public bool IsAuthenticated
        {
            get => GetAuthenticated();
            set => throw new NotImplementedException();
        }

        public string UserName
        {
            get => _httpContextAccessor.HttpContext.User.Identity.Name;
            set => throw new NotImplementedException();
        }

        public string FullName
        {
            get =>
                _httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.GivenName)
                    ?.Value + " " + _httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(c =>
                        c.Type ==
                        ClaimTypes
                            .Surname)
                    ?.Value;
            set => throw new NotImplementedException();
        }

        public string Email
        {
            get =>
                _httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            set => throw new NotImplementedException();
        }

        public string RemoteIpAddress
        {
            get => _httpContextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
            set => throw new NotImplementedException();
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _httpContextAccessor.HttpContext.User.Claims;
        }

        public IEnumerable<string> Roles => GetRoles();

        private int? GetUserId()
        {
            var userId =
                Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userId;
        }

        private bool GetAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        private string GetName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name ??
                   _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }

        private IEnumerable<string> GetRoles()
        {
            var claims = GetClaimsIdentity();
            var roles = new List<string>();
            foreach (var claim in claims)
                if (claim.Type == ClaimTypes.Role)
                    roles.Add(claim.Value);
            return roles;
        }
    }
}