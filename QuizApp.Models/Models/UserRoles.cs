
using QuizApp.WebAPI.Models;

namespace QuizApp.Models.Models;

public class UserRoles
{
    public Guid UserId { get; set; }
    public User? User { get; set; }  // Navigation property

    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}
