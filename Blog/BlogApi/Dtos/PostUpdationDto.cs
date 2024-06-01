using System.ComponentModel.DataAnnotations;

namespace BlogApi.Dtos
{
    public class PostUpdationDto
    {
        [Required(ErrorMessage = "The title is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the title is 100 characters.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The content is required.")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "The categories is required.")]
        public int[]? Categories { get; set; }

        [Required(ErrorMessage = "The status is required.")]
        [MaxLength(15, ErrorMessage = "Maximum length for the status is 15 characters.")]
        public string? Status { get; set; }
    }
}
