using System.ComponentModel.DataAnnotations;

namespace BlogApi.Dtos
{
    public class UserUpdationDto
    {
        [Required(ErrorMessage = "The username is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the username is 50 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "The firstName is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the firstName is 50 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "The lastName is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the lastName is 50 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "The emailAddress is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the emailAddress is 50 characters.")]
        [RegularExpression(
            @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$",
            ErrorMessage = "The email address is not valid"
        )]
        public string? EmailAddress { get; set; }
    }
}

