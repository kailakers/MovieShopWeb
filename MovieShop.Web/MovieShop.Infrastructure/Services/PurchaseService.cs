using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.Infrastructure.Services
{
    public class PurchaseService:IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ICurrentUserService _currentUserService;
        public PurchaseService(IPurchaseRepository purchaseRepository, ICurrentUserService currentUserService)
        {
            _purchaseRepository = purchaseRepository;
            _currentUserService = currentUserService;
        }
        public Task<IEnumerable<Purchase>> GetAllPurchases(int pageSize = 30, int pageIndex = 0)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Purchase>> GetAllPurchasesByMovie(int movieId, int pageSize = 30, int pageIndex = 0)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Purchase>> GetWeeklyPurchases()
        {
            var roles = _currentUserService.Roles;
            List<string> rolesList = new List<string>();
            foreach (var role in roles)
            {
                rolesList.Add(role);
            }

            string admin = "Admin";
            if (!rolesList.Exists(e=>e==admin))
                throw new Exception("Not Authorized");
            return await _purchaseRepository.GetWeeklyPurchases();
        }
    }
}