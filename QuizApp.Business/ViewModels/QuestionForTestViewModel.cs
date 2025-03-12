using System;

namespace QuizApp.Business.ViewModels;

public class QuestionForTestViewModel
{
    // Id, Content, QuestionType, Answers(AnswerForTestViewModel) 
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public string? QuestionType { get; set; }
    public List<AnswerForTestViewModel>? Answers { get; set; }
}
