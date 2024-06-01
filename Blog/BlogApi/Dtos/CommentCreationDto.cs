using System.ComponentModel.DataAnnotations;

namespace BlogApi.Dtos
{
    public class CommentCreationDto
    {
        [Required(ErrorMessage = "The userId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The content is required.")]
        public string? Content { get; set; }
    }
}
