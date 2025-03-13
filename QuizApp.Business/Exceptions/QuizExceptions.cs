namespace QuizApp.Business.Exceptions;

public class QuizExceptions
{
    public class QuizNotFoundException : Exception
    {
        public QuizNotFoundException(string message) : base(message) { }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message) { }
    }

    public class InvalidAnswerException : Exception
    {
        public InvalidAnswerException(string message) : base(message) { }
    }

    public class QuizSubmissionException : Exception
    {
        public QuizSubmissionException(string message) : base(message) { }
    }
}
