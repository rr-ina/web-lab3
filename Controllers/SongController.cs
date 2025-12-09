using lab3.DTOs;        
using lab3.Models;      
using lab3.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace lab3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
       private readonly ISongRepository _repository;
       private readonly IPlaylistRepository _playlistRepository;
        public SongController(ISongRepository repository, IPlaylistRepository playlistRepository)
        {
            _repository = repository;
            _playlistRepository = playlistRepository;
        }

        //getAll
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [HttpGet(Name = "GetAllSongs")]
        public async Task<ActionResult<IEnumerable<OutputSongDto>>> GetAll()
        {
            var songs = await _repository.GetAllAsync();

            var result = songs.Select(s => new OutputSongDto
            {
                Id = s.Id,
                Title = s.Title,
                Artist = s.Artist,
                DurationSeconds = s.DurationSeconds,
                PlaylistId = s.PlaylistId
            });

            return Ok(result);
        }

        //getAllById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [HttpGet("{id:int:min(1)}", Name = "GetSongById")]
        public async Task<ActionResult<OutputSongDto>> Get(int id)
        {
            var song = await _repository.GetByIdAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            var dto = new OutputSongDto
            {
                Id = song.Id,
                Title = song.Title,
                Artist = song.Artist,
                DurationSeconds = song.DurationSeconds,
                PlaylistId = song.PlaylistId
            };

            return Ok(dto);
        }

        //createSong
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [HttpPost(Name = "AddSong")]
        public async Task<ActionResult<OutputSongDto>> Post([FromBody] CreateSongDto songDto)
        {
           var playlist = await _playlistRepository.GetById(songDto.PlaylistId);
            
            if (playlist == null)
            {
                return NotFound($"Playlist with ID {songDto.PlaylistId} not found.");
            }

            var song = new Song
            {
                Title = songDto.Title!,
                Artist = songDto.Artist!,
                DurationSeconds = songDto.DurationSeconds,
                PlaylistId = songDto.PlaylistId
            };

            await _repository.AddAsync(song);

            var outputDto = new OutputSongDto
            {
                Id = song.Id,
                Title = song.Title,
                Artist = song.Artist,
                DurationSeconds = song.DurationSeconds,
                PlaylistId = song.PlaylistId
            };

            return CreatedAtAction(nameof(Get), new { id = song.Id }, outputDto);
        }

        //UpdateSongById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        [HttpPut("{id:int:min(1)}", Name = "UpdateSong")]
        public async Task<IActionResult> Put(int id, [FromBody] CreateSongDto songDto)
        {
            var existingSong = await _repository.GetByIdAsync(id);
            if (existingSong == null) return NotFound("Song not found");

            var playlist = await _playlistRepository.GetById(songDto.PlaylistId);
            if (playlist == null)
            {
                return NotFound($"Playlist with ID {songDto.PlaylistId} not found.");
            }

            existingSong.Title = songDto.Title!;
            existingSong.Artist = songDto.Artist!;
            existingSong.DurationSeconds = songDto.DurationSeconds;
            existingSong.PlaylistId = songDto.PlaylistId;

            await _repository.UpdateAsync(existingSong);

            return NoContent();
        }

        //deleteAll
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        [HttpDelete(Name = "DeleteAllSongs")]
        public async Task<IActionResult> DeleteAll()
        {
            await _repository.DeleteAll();
            return NoContent(); 
        }

        //deleteById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        [HttpDelete("{id:int:min(1)}", Name = "DeleteSong")]
        public async Task<IActionResult> Delete(int id)
        {
            var song = await _repository.GetByIdAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}