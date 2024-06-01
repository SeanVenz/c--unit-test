using BlogApi.Models;

namespace BlogApi.Dtos
{
    public class PostUserDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public UserDto? User { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public string? Status { get; set; }
        public string? DateTimeCreated { get; set; }
        public string? DateTimeUpdated { get; set; }
    }
}
