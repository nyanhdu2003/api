using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using QuizApp.Models.Models;

namespace QuizApp.WebAPI.Models;

public class Role : IdentityRole<Guid>, IBaseEntity
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public required string Description { get; set; }

    [Required]
    public required bool IsActive { get; set; } = true;

    // Relationship N:N with User
    public ICollection<User> Users { get; set; } = [];

    public ICollection<UserRoles>? UserRoles { get; set; }
}
