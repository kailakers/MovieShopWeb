using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieShop.Core.Models.Response;
using MovieShop.Core.RepositoryInterfaces;

namespace MovieShop.Infrastructure.Repositories
{
    public class ChartRepository:IChartRepository
    {
        private readonly IConfiguration _configuration;

        public ChartRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private SqlConnection Connection => new SqlConnection(_configuration.GetConnectionString("MovieShopDbConnection"));

        public async Task<IEnumerable<MovieChartResponseModel>> GetTopPurchasedMovies()
        {
            using (var con = CreateConnection())
            {
                var sql = @" SELECT p.MovieId, m.Title, COUNT(*) PurchaseCount
                             FROM [dbo].[Purchase] p
                                 JOIN Movie m on p.MovieId = m.Id
                             GROUP by p.MovieId, m.Title
                             ORDER by COUNT(*) DESC";

                var purchases = await con.QueryAsync<MovieChartResponseModel>(sql);
                return purchases;
            }
        }

        private IDbConnection CreateConnection()
        {
            var connection = Connection;
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                connection.Open();

            return connection;
        }
    }
}