using Microsoft.AspNetCore.Mvc;
using QuizApp.Business.Services;
using QuizApp.Business.ViewModels;

namespace QuizApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateQuestionWithAnswer(QuestionCreateViewModel questionCreateViewModel)
        {
            var result = await _questionService.CreateQuestionWithAnswerAsync(questionCreateViewModel);
            return result ? Ok() : BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            return question != null ? Ok(question) : NotFound();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return Ok(questions);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var result = await _questionService.DeleteQuestionAsync(id);
            return result ? Ok() : NotFound();
        }

        // Update Question
        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuestionWithAnswer(Guid id, QuestionEditViewModel questionEditViewModel)
        {
            var result = await _questionService.UpdateQuestionWithAnswerAsync(id, questionEditViewModel);
            return result ? Ok() : NotFound();
        }
    }
}
