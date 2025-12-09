namespace lab3.DTOs
{
    public class CreateSongDto
    {
        public string? Title { get; set; } 
        public string? Artist { get; set; }
        public int DurationSeconds { get; set; }
        public int PlaylistId { get; set; }
    }
}