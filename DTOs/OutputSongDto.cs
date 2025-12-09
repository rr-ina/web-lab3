namespace lab3.DTOs
{
    public class OutputSongDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public int DurationSeconds { get; set; }
        public int PlaylistId { get; set; }
    }
}