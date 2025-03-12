using System;
using QuizApp.Business.ViewModels;

namespace QuizApp.Business.Services;

public interface IQuizService
{
    Task<QuizPrepareInfoViewModel?> PrepareQuizForUserAsync(PrepareQuizViewModel prepareQuizViewModel);

    Task<QuizForTestViewModel> TakeQuizAsync(TakeQuizViewModel model);

    Task<bool> SubmitQuizAsync(QuizSubmissionViewModel model);

    Task<QuizResultViewModel> GetQuizResultAsync(GetQuizResultViewModel model);
}
