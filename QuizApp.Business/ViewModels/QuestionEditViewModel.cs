namespace QuizApp.Business.ViewModels;

public class QuestionEditViewModel
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<AnswerEditViewModel> Answers { get; set; } = [];
}
