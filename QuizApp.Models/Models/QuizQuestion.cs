using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuizApp.Models.Models;

namespace QuizApp.WebAPI.Models;

public class QuizQuestion : BaseEntity
{
    [ForeignKey(nameof(Quiz))]
    public Guid QuizId { get; set; }

    [Required]
    public required Quiz Quiz { get; set; }

    [ForeignKey(nameof(Question))]
    public Guid QuestionId { get; set; }

    public required Question Question { get; set; }

    public int Order { get; set; }
}
