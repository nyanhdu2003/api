using System.ComponentModel.DataAnnotations;

namespace QuizApp.Business.ViewModels;

public class QuizPrepareInfoViewModel
{
    // Id, Title, Description, Duration, ThumbnailUrl, QuizCode, User(UserViewModel)
    [Required]
    public required Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public required string Title { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public required int Duration { get; set; }

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    public string? QuizCode { get; set; }

    public UserViewModel? User { get; set; }
}
