using Microsoft.EntityFrameworkCore;
using lab3.Data;
using lab3.Models;
using Microsoft.Extensions.Caching.Memory;

namespace lab3.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PlaylistRepository> _logger;
        private readonly IMemoryCache _cache;

        public PlaylistRepository(AppDbContext context, ILogger<PlaylistRepository> logger, IMemoryCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<Playlist>> GetAll()
        {
            _logger.LogInformation("Запит на отримання всіх плейлистів.");

            string cacheKey = "AllPlaylists";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Playlist>? playlists))
            {
                _logger.LogInformation("Кеш пустий. Читаємо дані з бд.");

                playlists = await _context.Playlists.ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(15));

                _cache.Set(cacheKey, playlists, cacheOptions);
            }
            else
            {
                _logger.LogInformation("Дані успішно отримано з кешу");
            }

            return playlists;
        }

        public async Task<Playlist?> GetById(int id)
        {
            _logger.LogInformation($"Пошук плейлиста з ID: {id}");
            return await _context.Playlists.FindAsync(id);
        }

        public async Task Add(Playlist playlist)
        {
            _logger.LogInformation($"Спроба створення плейлиста: '{playlist.Name}'");

            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync();

            _cache.Remove("AllPlaylists");
            _logger.LogInformation($"Плейлист '{playlist.Name}' успішно створено. Присвоєно ID: {playlist.Id}");
        }

        public async Task Update(Playlist playlist)
        {
            _logger.LogInformation($"Оновлення даних плейлиста з ID: {playlist.Id}");

            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();

            _cache.Remove("AllPlaylists");
            _logger.LogInformation($"Плейлист ID: {playlist.Id} успішно оновлено.");
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation($"Запит на видалення плейлиста з ID: {id}");

            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();

                _cache.Remove("AllPlaylists");
                _logger.LogInformation($"Плейлист ID: {id} було успішно видалено з бази даних.");
            }
            else
            {
                _logger.LogWarning($"Плейлист з ID {id} не знайдено.");
            }
        }
    }
}