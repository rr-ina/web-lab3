using Microsoft.EntityFrameworkCore; 
using lab3.Data;
using lab3.Models;

namespace lab3.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly AppDbContext _context;

        public PlaylistRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Playlist>> GetAll()
        {
            return await _context.Playlists.ToListAsync();
        }

        public async Task<Playlist?> GetById(int id)
        {
            return await _context.Playlists.FindAsync(id);
        }

        public async Task Add(Playlist playlist)
        {
            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync(); 
        }

        public async Task Update(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
            }
        }
    }
}