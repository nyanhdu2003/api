using System.ComponentModel.DataAnnotations;

namespace QuizApp.WebAPI.Models;

public class Question
{
    [Required]
    public required Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(5000, MinimumLength = 5)]
    public required string Content { get; set; }

    [Required]
    [EnumDataType(typeof(QuestionType))]
    public required string QuestionType { get; set; }

    [Required]
    public required bool IsActive { get; set; } = true;

    // Question - Answer: 1:N 
    public ICollection<Answer> Answers { get; set; } = [];

    public ICollection<QuizQuestion> QuizQuestions { get; set; } = [];

}
