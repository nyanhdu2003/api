using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using QuizApp.Models.Models;

namespace QuizApp.WebAPI.Models;

public class User : IdentityUser<Guid>, IBaseEntity
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public required string LastName { get; set; }

    [NotMapped]
    public string? DisplayName { get; set; }

    [Required]
    [CustomValidation(typeof(User), nameof(ValidateDateOfBirth))]
    public DateTime DateOfBirth { get; set; }

    [StringLength(500)]
    public string? Avatar { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    public static ValidationResult? ValidateDateOfBirth(DateTime dateOfBirth, ValidationContext context)
    {
        if (dateOfBirth > DateTime.Now)
        {
            return new ValidationResult("Date of birth cannot be in the future.");
        }
        return ValidationResult.Success;
    }

    // Relationship N:N with Role
    public ICollection<Role> Roles { get; set; } = [];

    // Relationship N:N with Quiz
    public ICollection<UserQuizz> UserQuizzes { get; set; } = [];

    public ICollection<UserRoles>? UserRoles { get; set; }

}
