using System.ComponentModel.DataAnnotations;

namespace QuizApp.WebAPI.Models;

public class Answer
{
    [Required]
    public required Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(255, MinimumLength = 5)]
    public required string Content { get; set; }

    [Required]
    public required bool IsCorrect { get; set; } = false;
}
