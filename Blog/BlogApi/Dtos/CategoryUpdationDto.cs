using System.ComponentModel.DataAnnotations;

namespace BlogApi.Dtos
{
    public class CategoryUpdationDto
    {
        [Required(ErrorMessage = "The name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the name is 50 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The description is required.")]
        [MaxLength(250, ErrorMessage = "Maximum length for the description is 250 characters.")]
        public string? Description { get; set; }
    }
}
