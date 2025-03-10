using QuizApp.WebAPI.Data;
using QuizApp.WebAPI.Models;

namespace QuizApp.WebAPI.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly QuizAppDbContext _context;

    public QuizRepository(QuizAppDbContext context)
    {
        _context = context;
    }

    public int Add(Quiz entity)
    {
        _context.Quizzes.Add(entity);
        return _context.SaveChanges();
    }

    public bool Delete(Guid Id)
    {
        var entity = _context.Quizzes.FirstOrDefault(q => q.Id == Id);
        if (entity == null) return false;
        _context.Quizzes.Remove(entity);
        return _context.SaveChanges() > 0;
    }

    public bool Delete(Quiz entity)
    {
        _context.Quizzes.Remove(entity);
        return _context.SaveChanges() > 0;
    }

    public IEnumerable<Quiz> GetAll()
    {
        return [.. _context.Quizzes];
    }

    public Quiz? GetById(Guid Id)
    {
        return _context.Quizzes.FirstOrDefault(q => q.Id == Id);
    }

    public bool Update(Quiz entity)
    {
        var existingQuiz = _context.Quizzes.Find(entity.Id);
        if (existingQuiz == null) return false;

        _context.Entry(existingQuiz).CurrentValues.SetValues(entity);
        return _context.SaveChanges() > 0;
    }
}
