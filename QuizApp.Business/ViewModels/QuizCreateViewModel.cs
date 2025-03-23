namespace QuizApp.Business.ViewModels;

public class QuizCreateViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Duration { get; set; }
    public bool IsActive { get; set; }
    public ICollection<QuestionIdWithOrderViewModel> QuestionIdWithOrders { get; set; } = [];
}
