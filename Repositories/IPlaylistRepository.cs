using lab3.Models;

namespace lab3.Repositories
{
    public interface IPlaylistRepository
    {
        Task<IEnumerable<Playlist>> GetAll();
        Task<Playlist?> GetById(int id);
        Task Add(Playlist playlist);
        Task Update(Playlist playlist);
        Task Delete(int id);
    }
}