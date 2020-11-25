using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MovieShop.Core.Entities;
using MovieShop.Core.MappingProfiles;
using MovieShop.Core.Models.Response;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Services;
using NUnit.Framework;

namespace MovieShop.UnitTests
{
    [TestFixture]
    public class MovieServiceTest
    {
        private MovieService _sut;

        private Mock<IMovieRepository> _mockMovieRepository;
        private Mock<IAsyncRepository<MovieGenre>> _mockGenresRepository;
        private Mock<IPurchaseRepository> _mockPurchaseRepository;
        private Mock<IAsyncRepository<Favorite>> _mockFavoriteRepository;
        private Mapper _mapper;

        private List<Movie> _movies;


        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _movies = new List<Movie>
                      {
                          new Movie {Id = 1, Title = "Avengers: Infinity War", Budget = 1200000},
                          new Movie {Id = 2, Title = "Avatar", Budget = 1200000},
                          new Movie {Id = 3, Title = "Star Wars: The Force Awakens", Budget = 1200000},
                          new Movie {Id = 4, Title = "Titanic", Budget = 1200000},
                          new Movie {Id = 5, Title = "Inception", Budget = 1200000},
                          new Movie {Id = 6, Title = "Avengers: Age of Ultron", Budget = 1200000},
                          new Movie {Id = 7, Title = "Interstellar", Budget = 1200000},
                          new Movie {Id = 8, Title = "Fight Club", Budget = 1200000},
                          new Movie
                          {
                              Id = 9, Title = "The Lord of the Rings: The Fellowship of the Ring", Budget = 1200000
                          },
                          new Movie {Id = 10, Title = "The Dark Knight", Budget = 1200000},
                          new Movie {Id = 11, Title = "The Hunger Games", Budget = 1200000},
                          new Movie {Id = 12, Title = "Django Unchained", Budget = 1200000},
                          new Movie
                          {
                              Id = 13, Title = "The Lord of the Rings: The Return of the King", Budget = 1200000
                          },
                          new Movie {Id = 14, Title = "Harry Potter and the Philosopher's Stone", Budget = 1200000},
                          new Movie {Id = 15, Title = "Iron Man", Budget = 1200000},
                          new Movie {Id = 16, Title = "Furious 7", Budget = 1200000}
                      };
        }

        [SetUp]
        public void SetUp()
        {
            var myProfile = new MoviesMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _mockMovieRepository = new Mock<IMovieRepository>();
            _mockGenresRepository = new Mock<IAsyncRepository<MovieGenre>>();
            _mockPurchaseRepository = new Mock<IPurchaseRepository>();
            _mockFavoriteRepository = new Mock<IAsyncRepository<Favorite>>();

            _sut = new MovieService(_mockMovieRepository.Object, _mapper, _mockGenresRepository.Object,
                                    _mockPurchaseRepository.Object,
                                    _mockFavoriteRepository.Object);

            _mockMovieRepository.Setup(m => m.GetHighestGrossingMovies()).ReturnsAsync(_movies);
        }

       



        [Test]
        public async Task TestListOfMoviesFromFakeData()
        {
            var movies = await _sut.GetHighestGrossingMovies();

            Assert.NotNull(movies);
            Assert.That(movies.Count(), Is.EqualTo(16));
            CollectionAssert.AllItemsAreInstancesOfType(movies, typeof(MovieResponseModel));

            
        }
    }
}