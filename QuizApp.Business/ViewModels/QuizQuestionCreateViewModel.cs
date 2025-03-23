using System;

namespace QuizApp.Business.ViewModels;

public class QuizQuestionCreateViewModel
{   
    public Guid QuizId { get; set; }

    public Guid QuestionId { get; set; }

    public int Order { get; set; }


}
