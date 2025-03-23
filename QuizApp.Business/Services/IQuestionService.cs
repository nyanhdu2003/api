using QuizApp.Business.ViewModels;

namespace QuizApp.Business.Services;

public interface IQuestionService
{
    // get quesion by id async
    Task<QuestionViewModel?> GetQuestionByIdAsync(Guid id);

    // get all questions async
    Task<IEnumerable<QuestionViewModel>> GetAllQuestionsAsync();

    // create question async
    Task<bool> CreateQuestionWithAnswerAsync(QuestionCreateViewModel model);

    // Update an existing question by ID with updated answers
    Task<bool> UpdateQuestionWithAnswerAsync(Guid id, QuestionEditViewModel model);

    // Delete a question by ID (consider associated answers before deletion)
    Task<bool> DeleteQuestionAsync(Guid id);
}