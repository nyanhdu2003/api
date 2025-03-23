using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuizApp.Models.Models;

namespace QuizApp.WebAPI.Models;

public class UserQuizz : BaseEntity
{
    [Required]
    [ForeignKey(nameof(User))]
    public required Guid UserId { get; set; }
    public User? User { get; set; }

    [Required]
    [ForeignKey(nameof(Quiz))]
    public required Guid QuizId { get; set; }
    public Quiz? Quiz { get; set; }

    [Required]
    [StringLength(10)]
    public required string QuizCode { get; set; }

    public DateTime? StartAt { get; set; }

    public DateTime? FinishAt { get; set; }

    public ICollection<UserAnswer> UserAnswers { get; set; } = [];
}
