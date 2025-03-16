using Microsoft.AspNetCore.Mvc;
using QuizApp.Business.Services;
using QuizApp.Business.ViewModels;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;
[Route("api/[controller]")]
[ApiController]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpPost("prepare")]
    public async Task<IActionResult> PrepareQuiz([FromBody] PrepareQuizViewModel model)
    {
        var result = await _quizService.PrepareQuizForUserAsync(model);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost("take")]
    public async Task<IActionResult> TakeQuiz([FromBody] TakeQuizViewModel model)
    {
        var quiz = await _quizService.TakeQuizAsync(model);
        return Ok(quiz);
    }

    [HttpPost("submit")]
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
}
