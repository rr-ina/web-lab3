using lab3.Models;
using lab3.Repositories;
using Microsoft.AspNetCore.Mvc;
using lab3.DTOs;

namespace lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _repository;

        public PlaylistController(IPlaylistRepository repository)
        {
            _repository = repository;
        }

        //getAll
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [HttpGet(Name = "GetAllPlaylists")]
       public async Task<ActionResult<IEnumerable<OutputPlaylistDto>>> GetAll()
        {
            var playlists = await _repository.GetAll();

            var result = playlists.Select(p => new OutputPlaylistDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                IsPublic = p.IsPublic
            });

            return Ok(result);
        }

        //getAllById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
       [HttpGet("{id:int:min(1)}", Name = "GetPlaylistById")]
        public async Task<ActionResult<OutputPlaylistDto>> Get(int id)
        {
            var playlist = await _repository.GetById(id);
            if (playlist == null) return NotFound();

            var dto = new OutputPlaylistDto
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic
            };

            return Ok(dto);
        }

        //createPlaylist
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [HttpPost(Name = "AddPlaylist")]
        public async Task<ActionResult<OutputPlaylistDto>> Post([FromBody] CreatePlaylistDto dto)
        {
            var playlist = new Playlist
            {
                Name = dto.Name!,
                Description = dto.Description!,
                IsPublic = dto.IsPublic
            };

            await _repository.Add(playlist);

            var outputDto = new OutputPlaylistDto
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic
            };

            return CreatedAtAction(nameof(Get), new { id = playlist.Id }, outputDto);
        }

        //UpdateSongById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        [HttpPut("{id:int:min(1)}", Name = "UpdatePlaylist")]
        public async Task<IActionResult> Put(int id, [FromBody] CreatePlaylistDto dto)
        {
            var existingPlaylist = await _repository.GetById(id);
            if (existingPlaylist == null) return NotFound();

            existingPlaylist.Name = dto.Name!;
            existingPlaylist.Description = dto.Description!;
            existingPlaylist.IsPublic = dto.IsPublic;

            await _repository.Update(existingPlaylist);

            return NoContent();
        }

        //deleteById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        [HttpDelete("{id:int:min(1)}", Name = "DeletePlaylist")]
        public async Task<IActionResult> Delete(int id)
        {
            var playlist = await _repository.GetById(id);
            if (playlist == null) return NotFound();

            await _repository.Delete(id);
            return NoContent();
        }
    }
}