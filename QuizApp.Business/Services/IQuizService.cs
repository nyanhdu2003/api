using System;
using QuizApp.Business.ViewModels;

namespace QuizApp.Business.Services;

public interface IQuizService
{

    Task<QuizForTestViewModel> TakeQuizAsync(TakeQuizViewModel model);

    Task<bool> SubmitQuizAsync(QuizSubmissionViewModel model);

    Task<QuizResultViewModel> GetQuizResultAsync(GetQuizResultViewModel model);

    Task<QuizViewModel> GetQuizByIdAsync(Guid id);

    Task<List<QuizViewModel>> GetAllQuizzesAsync();

    Task<bool> CreateQuizWithQuestionsAsync(QuizCreateViewModel model);

    Task<bool> UpdateQuizWithQuestionsAsync(Guid id, QuizEditViewModel model);

    Task<bool> DeleteQuizAsync(Guid id);

    Task<bool> AddQuestionToQuiz(QuizQuestionCreateViewModel model);

    Task<bool> DeleteQuestionFromQuiz(Guid id, Guid questionId);

    Task<QuizPrepareInfoViewModel?> PrepareQuizForUser(PrepareQuizViewModel prepareQuizViewModel);
}
