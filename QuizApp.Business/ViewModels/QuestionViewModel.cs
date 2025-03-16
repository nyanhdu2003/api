namespace QuizApp.Business.ViewModels;

public class QuestionViewModel
{   
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public string? QuestionType { get; set; }
    public bool IsActive { get; set; }
}
