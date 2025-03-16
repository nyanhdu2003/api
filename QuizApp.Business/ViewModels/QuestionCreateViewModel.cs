using QuizApp.Business.ViewModels;

namespace QuizApp.Business.Services;

public class QuestionCreateViewModel
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<AnswerCreateViewModel> Answers { get; set; } = [];
}
