namespace QuizApp.Business.ViewModels;
using System.Collections.Generic;

public class QuizEditViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }= string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Duration { get; set; }
    public bool IsActive { get; set; }
    public ICollection<QuestionIdWithOrderViewModel> QuestionIdWithOrders { get; set; } = [];
}
