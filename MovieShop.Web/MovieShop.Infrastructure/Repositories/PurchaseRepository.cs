using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;

namespace MovieShop.Infrastructure.Repositories
{
    public class PurchaseRepository : EfRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<Purchase>> GetAllPurchases(int pageSize = 30, int pageIndex = 0)
        {
            return await _dbContext.Purchases.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<Purchase>> GetWeeklyPurchases()
        {
            var purchases = await _dbContext.Purchases
                .Where(p => p.PurchaseDateTime > (DateTime.Now.AddDays(-7)))
                .OrderByDescending(p => p.PurchaseDateTime)
                .ToListAsync();
            return purchases;
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesByUserId(int id)
        {
            var purchases = await _dbContext.Purchases.Where(p => p.UserId == id)
                .OrderByDescending(p => p.PurchaseDateTime)
                .ToListAsync();
            return purchases;
        }

        public async Task<IEnumerable<Purchase>> GetAllPurchasesByMovie(int movieId, int pageSize = 30, int pageIndex = 0)
        {
            return await _dbContext.Purchases.Where(p => p.MovieId == movieId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}