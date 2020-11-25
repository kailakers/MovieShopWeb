using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MovieShop.Core.RepositoryInterfaces;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace MovieShop.API.WorkerService
{
    public class ChartRecurringService : IChartRecurringService
    {
        private readonly IChartRepository _chartRepository;
        private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromMinutes(1);
        private readonly IMemoryCache _cache;
        private readonly ILogger<ChartRecurringService> _logger;
        private static readonly string _chartsKey = "chartsData";

        public ChartRecurringService(IChartRepository chartRepository, IMemoryCache cache,
            ILogger<ChartRecurringService> logger)
        {
            _chartRepository = chartRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> ExecuteAsync<T>(T args)
        {
            var jobType = "ChartServiceJob";
            BackgroundJob.Enqueue(() => GetPopularMovies());
            return await Task.FromResult(jobType);
        }

        public async Task GetPopularMovies()
        {
            var movies = await _cache.GetOrCreateAsync(_chartsKey, async entry =>
            {
                entry.SlidingExpiration = _defaultCacheDuration;
                return await _chartRepository.GetTopPurchasedMovies();
            });
            _logger.LogInformation($"Top Movie is {movies.FirstOrDefault()?.Title} ");
        }
    }

    public interface IChartRecurringService
    {
        Task<string> ExecuteAsync<T>(T args);
    }
}