using Microsoft.AspNetCore.Mvc;
using QuizApp.Business.Services;
using QuizApp.Business.ViewModels;

namespace QuizApp.WebAPI.Controllers
{
    [Route("api/Quiz")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost("prepareQuizForUser")]
        public async Task<IActionResult> PrepareQuizForUser([FromBody] PrepareQuizViewModel prepareQuizViewModel)
        {
            var result = await _quizService.PrepareQuizForUser(prepareQuizViewModel);
            return result == null ? BadRequest("Invalid data.") : Ok(result);
        }

        [HttpPost("takeQuiz")]
        public async Task<IActionResult> TakeQuiz([FromBody] TakeQuizViewModel model)
        {
            var quiz = await _quizService.TakeQuizAsync(model);
            return Ok(quiz);
        }

        [HttpPost("submitQuiz")]
        public async Task<IActionResult> SubmitQuiz([FromBody] QuizSubmissionViewModel model)
        {
            var success = await _quizService.SubmitQuizAsync(model);
            return success ? Ok() : BadRequest();
        }

        [HttpPost("result")]
        public async Task<IActionResult> GetResult([FromBody] GetQuizResultViewModel model)
        {
            var result = await _quizService.GetQuizResultAsync(model);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuizWithQuestions(Guid id, QuizEditViewModel quizEditViewModel)
        {
            var result = await _quizService.UpdateQuizWithQuestionsAsync(id, quizEditViewModel);
            return Ok(result);
        }

        [HttpGet("quizzes/{id}")]
        public async Task<IActionResult> GetQuizByIdAsync(Guid id)
        {
            var question = await _quizService.GetQuizByIdAsync(id);
            return question != null ? Ok(question) : NotFound();
        }

        [HttpGet("quizzes")]
        public async Task<IActionResult> GetAllQuestionsAsync()
        {
            var questions = await _quizService.GetAllQuizzesAsync();
            return Ok(questions);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            var result = await _quizService.DeleteQuizAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpPost("createQuizzes")]
        public async Task<IActionResult> CreateQuizWithQuestions(QuizCreateViewModel quizCreateViewModel)
        {
            var result = await _quizService.CreateQuizWithQuestionsAsync(quizCreateViewModel);
            return result ? Ok() : BadRequest();
        }

        [HttpPost("addQuestionToQuiz")]
        public async Task<IActionResult> AddQuestionToQuiz(QuizQuestionCreateViewModel quizQuestionCreateViewModel)
        {
            var result = await _quizService.AddQuestionToQuiz(quizQuestionCreateViewModel);
            return result ? Ok() : BadRequest();
        }

        [HttpDelete("{id}/questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestionFromQuiz(Guid id, Guid questionId)
        {
            var result = await _quizService.DeleteQuestionFromQuiz(id, questionId);
            return result ? Ok() : BadRequest();
        }
    }
}
