using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;

namespace MovieShop.Infrastructure.Repositories
{
    public class UserRepository: EfRepository<User>, IUserRepository
    {
        public UserRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users.Include(u=>u.UserRoles).ThenInclude(ur=>ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}