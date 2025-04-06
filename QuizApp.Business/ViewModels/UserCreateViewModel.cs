
using System.ComponentModel.DataAnnotations;

namespace QuizApp.Business.Services;

public class UserCreateViewModel
{
    //FirstName, LastName, Email, UserName, PhoneNumber, Password, ConfirmPassword, DateOfBirth, IsActive.
    public Guid Id { get; set; }

    [Required]
    [StringLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public required string LastName { get; set; }

    public string? Email { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required]
    public required string ConfirmPassword { get; set; }

    public string? UserName { get; set; }

    [StringLength(15)]
    public string? PhoneNumber { get; set; }

    [Required]
    public required DateTime DateOfBirth { get; set; }

    public bool IsActive { get; set; }
}