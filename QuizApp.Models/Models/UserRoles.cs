
using Microsoft.AspNetCore.Identity;
using QuizApp.WebAPI.Models;

namespace QuizApp.Models.Models;

public class UserRoles : IdentityUserRole<Guid>
{
    
    public User? User { get; set; }  // Navigation property
    public Role? Role { get; set; }
}
