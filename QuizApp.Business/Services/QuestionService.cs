using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using QuizApp.Business.ViewModels;
using QuizApp.WebAPI.Data;
using QuizApp.WebAPI.Models;

namespace QuizApp.Business.Services;

public class QuestionService : IQuestionService
{
    private readonly QuizAppDbContext _context;

    public QuestionService(QuizAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateQuestionWithAnswerAsync(QuestionCreateViewModel model)
    {
        var question = new Question
        {
            Id = Guid.NewGuid(),
            Content = model.Content,
            QuestionType = model.QuestionType,
            IsActive = model.IsActive,
            Answers = model.Answers.Select(a => new Answer
            {
                Id = Guid.NewGuid(),
                Content = a.Content,
                IsCorrect = a.IsCorrect,
                IsActive = a.IsActive
            }).ToList()
        };

        _context.Questions.Add(question);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteQuestionAsync(Guid id)
    {
        var question = await _context.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
            return false;

        _context.Answers.RemoveRange(question.Answers);
        _context.Questions.Remove(question);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<QuestionViewModel>> GetAllQuestionsAsync()
    {
        return await _context.Questions
            .Select(q => new QuestionViewModel
            {
                Id = q.Id,
                Content = q.Content,
                QuestionType = q.QuestionType,
                IsActive = q.IsActive
            })
            .ToListAsync();
    }

    public async Task<QuestionViewModel?> GetQuestionByIdAsync(Guid id)
    {
        return await _context.Questions
            .Where(q => q.Id == id)
            .Select(q => new QuestionViewModel
            {
                Id = q.Id,
                Content = q.Content,
                QuestionType = q.QuestionType,
                IsActive = q.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateQuestionWithAnswerAsync(Guid id, QuestionEditViewModel model)
    {
        var question = await _context.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
        {
            Console.WriteLine("Question not found");
            return false;
        }
        
        question.Content = model.Content;
        question.QuestionType = model.QuestionType;
        question.IsActive = model.IsActive;

        var answers = await _context.Answers
            .Where(a => a.QuestionId == id)
            .ToListAsync();

        if (model.Answers != null && model.Answers.Count > 0)
        {
            _context.Answers.RemoveRange(answers);

            foreach (var answer in model.Answers)
            {
                var updatedAnswer = new Answer
                {
                    Id = Guid.NewGuid(),
                    Content = answer.Content,
                    IsCorrect = answer.IsCorrect,
                    IsActive = answer.IsActive,
                    QuestionId = question.Id
                };
                await _context.Answers.AddAsync(updatedAnswer);
                await _context.SaveChangesAsync();
            }
        }
        return await _context.SaveChangesAsync() > 0;
    }
}

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