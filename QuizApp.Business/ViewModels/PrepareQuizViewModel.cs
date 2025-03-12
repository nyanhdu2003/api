using System.ComponentModel.DataAnnotations;

namespace QuizApp.Business.ViewModels;

public class PrepareQuizViewModel
{
    // QuizId, UserId, QuizCode
    [Required]
    public required Guid QuizId { get; set; }

    [Required]
    public required Guid UserId { get; set; }

    public string? QuizCode { get; set; }
}
