using BlogApi.Models;

namespace BlogApi.Dtos
{
    public class UserPostDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? DateTimeCreated { get; set; }
        public List<PostDto> Posts { get; set; } = new List<PostDto>();
    }
}
