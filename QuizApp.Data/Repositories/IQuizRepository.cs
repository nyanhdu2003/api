using QuizApp.WebAPI.Models;

namespace QuizApp.WebAPI.Repositories;

public interface IQuizRepository
{
    IEnumerable<Quiz> GetAll();

    Quiz? GetById(Guid Id);

    int Add(Quiz entity);

    bool Update(Quiz entity);

    bool Delete(Guid Id);
    
    bool Delete(Quiz entity);
}