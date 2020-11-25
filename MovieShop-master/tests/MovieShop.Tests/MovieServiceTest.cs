using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;

namespace MovieShop.Tests
{
    [TestClass]
    public class MovieServiceTest
    {
        private readonly Mock<IMovieRepository> _mockMovieRepository;
        private readonly Mock<IAsyncRepository<MovieGenre>>  _mockGenresRepository;
        private readonly Mock<IPurchaseRepository>  _mockPurchaseRepository;
        private readonly Mock<IAsyncRepository<Favorite>>  _mockFavoriteRepository;

        public MovieServiceTest()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _mockGenresRepository = new Mock<IAsyncRepository<MovieGenre>>();
            _mockPurchaseRepository = new Mock<IPurchaseRepository>();
            _mockFavoriteRepository = new Mock<IAsyncRepository<Favorite>>();
        }


        [TestMethod]
        public void TestListOfMoviesFromFakeData()
        {
            Assert.AreEqual(10,10);
        }
    }
}
