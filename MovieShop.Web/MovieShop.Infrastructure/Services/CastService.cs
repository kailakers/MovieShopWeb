using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieShop.Core.Entities;
using MovieShop.Core.Models;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using MovieShop.Infrastructure.Migrations;

namespace MovieShop.Infrastructure.Services
{
    public class CastService:ICastService
    {
        private readonly ICastRepository _castRepository;

        public CastService(ICastRepository castRepository)
        {
            _castRepository = castRepository;
        }
        public async Task<CastDetailsResponseModel> GetCastDetailsById(int id)
        {
            var cast = await _castRepository.GetCastDetailById(id);
            if (cast == null)
            {
                throw new Exception();
            }

            List<MovieResponseModel> movieResponseModel = new List<MovieResponseModel>();
            foreach (var movieCast in cast.MovieCasts)
            {
                movieResponseModel.Add(new MovieResponseModel
                {
                    Id = movieCast.MovieId,
                    Title = movieCast.Movie.Title,
                    PosterUrl = movieCast.Movie.PosterUrl,
                    ReleaseDate = movieCast.Movie.ReleaseDate
                });
            }
            CastDetailsResponseModel castDetailsResponseModel = new CastDetailsResponseModel
            {
                Id = cast.Id,
                Gender = cast.Gender,
                ProfilePath = cast.ProfilePath,
                Name = cast.Name,
                TmdbUrl = cast.TmdbUrl,
                Movies = movieResponseModel
            };
            return castDetailsResponseModel;
        }
    }
}