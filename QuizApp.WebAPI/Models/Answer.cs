using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [Required]
    public required bool IsActive { get; set; } = true;

    // Foreign Key - Answer-Question: 1:1
    [Required]
    [ForeignKey("Question")]
    public Guid QuestionId { get; set; }

    public Question? Question { get; set; }
}
