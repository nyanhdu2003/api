using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuizApp.Models.Models;

namespace QuizApp.WebAPI.Models;

public class UserAnswer : BaseEntity
{
    [Required]
    public required Guid UserId { get; set; }

    [Required]
    public required Guid QuizId { get; set; }

    [ForeignKey("UserId, QuizId")]
    public UserQuizz? UserQuizz { get; set; }

    [Required]
    [ForeignKey(nameof(Answer))]
    public required Guid AnswerId { get; set; }

    public Answer? Answer { get; set; }

    [Required]
    [ForeignKey(nameof(Question))]
    public required Guid QuestionId { get; set; }

    public Question? Question { get; set; }

    [Required]
    public required bool IsCorrect { get; set; }
}
