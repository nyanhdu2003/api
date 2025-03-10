using System.ComponentModel.DataAnnotations;

namespace QuizApp.WebAPI.Models;

public class Quiz
{
    [Required]
    public required Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(255, MinimumLength = 5)]
    public required string Title { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(1, 3600)]
    public required int Duration { get; set; }

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    [Required]
    public required bool IsActive { get; set; } = true;

    // Relationship N:N with User
    public ICollection<UserQuiz> UserQuizzes { get; set; } = [];

    public ICollection<QuizQuestion> QuizQuestions { get; set; } = [];

}
