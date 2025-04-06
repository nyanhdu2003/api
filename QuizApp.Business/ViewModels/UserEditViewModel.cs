using System.ComponentModel.DataAnnotations;

namespace QuizApp.Business.Services;

public class UserEditViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime? DateOfBirth { get; set; } 
}
