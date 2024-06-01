using System.ComponentModel.DataAnnotations;

namespace BlogApi.Dtos
{
    public class PostGetDto
    {
        [MaxLength(15, ErrorMessage = "Maximum length for the status is 15 characters.")]
        public string? Status { get; set; }
    }
}
