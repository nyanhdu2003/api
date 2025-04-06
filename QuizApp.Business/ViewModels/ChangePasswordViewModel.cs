using System.ComponentModel.DataAnnotations;

namespace QuizApp.Business.Services;

public class ChangePasswordViewModel
{
    [Required]
    public required Guid Id { get; set; }

    public string? UserName { get; set; }

    [Required]
    public required string CurrentPassword { get; set; }
    
    [Required]
    public required string NewPassword { get; set; }
    
    [Required]
    public required string ConfirmPassword { get; set; }
    


}