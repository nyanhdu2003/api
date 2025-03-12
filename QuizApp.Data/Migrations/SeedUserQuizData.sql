INSERT INTO UserQuizz (Id, UserId, QuizId, QuizCode, StartAt, FinishAt)
VALUES 
  (NEWID(), '31169d5e-5cf5-4100-a15d-4100dc23dabc', 'e68f937a-1208-4564-9957-1279fb900c41', 'MATH001', GETDATE(), NULL),
  (NEWID(), '0cde6b1c-550b-40d1-b2b5-5d081e197aa9', 'd0e18412-ddb3-4305-b813-1558bbc3ec8e', 'HIST001', GETDATE(), NULL);
