using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace QuizApp.WebAPI.Models;

public class Role : IdentityRole<Guid>
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public required string Description { get; set; }

    [Required]
    public required bool IsActive { get; set; } = true;
}
