using System.ComponentModel.DataAnnotations;

namespace QuizApp.Business.ViewModels;

public class UserViewModel
{
    //Id, FirstName, LastName, DisplayName, Email, UserName, PhoneNumber, DateOfBirth, Avatar, IsActive, Roles

    [Required]
    public required Guid Id { get; set; }

    [Required]
    [StringLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public required string LastName { get; set; }

    [Required]
    [StringLength(100)]
    public required string DisplayName { get; set; }

    public string? Email { get; set; }

    public string? UserName { get; set; }

    [StringLength(15)]
    public string? PhoneNumber { get; set; }

    [Required]
    public required DateTime DateOfBirth { get; set; }

    public string? Avatar { get; set; }

    public bool IsActive { get; set; }

    public List<string?>? Roles { get; set; }
}
