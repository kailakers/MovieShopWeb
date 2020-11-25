using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MovieShop.Core.Entities;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.API.Caching
{
    public class CachedGenreService : ICachedGenreService
    {
        private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromDays(30);
        private static readonly string _genresKey = "genres";
        private readonly IMemoryCache _cache;
        private readonly IGenreService _genreService;

        public CachedGenreService(IMemoryCache cache, IGenreService genreService)
        {
            _cache = cache;
            _genreService = genreService;
        }

        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            var genres = await _cache.GetOrCreateAsync(_genresKey, async entry =>
            {
                entry.SlidingExpiration = _defaultCacheDuration;
                return await _genreService.GetAllGenres();
            });
            return genres;
        }
    }

    public interface ICachedGenreService
    {
        Task<IEnumerable<Genre>> GetAllGenres();
    }
}