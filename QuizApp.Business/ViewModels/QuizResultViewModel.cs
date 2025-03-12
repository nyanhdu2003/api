namespace QuizApp.Business.Services;

public class QuizResultViewModel
{
    public Guid QuizId { get; set; }
    public Guid UserId { get; set; }    
    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
    public int Score { get; set; }
}