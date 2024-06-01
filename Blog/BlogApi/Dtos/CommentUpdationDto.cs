using System.ComponentModel.DataAnnotations;

namespace BlogApi.Dtos
{
    public class CommentUpdationDto
    {
        [Required(ErrorMessage = "The content is required.")]
        public string? Content { get; set; }
    }
}
