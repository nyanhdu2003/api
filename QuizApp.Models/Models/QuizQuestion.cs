using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuizApp.Models.Models;

namespace QuizApp.WebAPI.Models;

public class QuizQuestion : BaseEntity
{
    [ForeignKey(nameof(Quiz))]
    [Required]
    public Guid QuizId { get; set; }

    public  Quiz? Quiz { get; set; }

    [ForeignKey(nameof(Question))]
    [Required]
    public Guid QuestionId { get; set; }

    public  Question? Question { get; set; }

    public int Order { get; set; }
}
