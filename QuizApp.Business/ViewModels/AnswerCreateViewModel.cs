using System;

namespace QuizApp.Business.ViewModels;

public class AnswerCreateViewModel
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public bool IsActive { get; set; }
}
