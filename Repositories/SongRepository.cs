using Microsoft.EntityFrameworkCore;
using lab3.Data;
using lab3.Models;

namespace lab3.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SongRepository> _logger;

        public SongRepository(AppDbContext context, ILogger<SongRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Song>> GetAllAsync()
        {
            return await _context.Songs.ToListAsync();
        }

        public async Task<Song?> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Пошук пісні за ID: {id}");

            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                _logger.LogWarning($"Пісню з ID {id} не знайдено в базі даних.");
            }

            return song;
        }

        public async Task AddAsync(Song song)
        {
            _logger.LogInformation($"Спроба додати пісню '{song.Title}'");

            try
            {
                await _context.Songs.AddAsync(song);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Пісню успішно додано з ID: {song.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Помилка при додаванні пісні '{song.Title}'.");

                throw; 
            }
        }

        public async Task UpdateAsync(Song song)
        {
            _context.Songs.Update(song);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
            else 
            {
                _logger.LogWarning($"Спроба видалити неіснуючу пісню ID: {id}");
            }
        }

        public async Task DeleteAll()
        {
            _logger.LogWarning("Виконується повне видалення пісень!");
            await _context.Songs.ExecuteDeleteAsync();
        }
    }
}