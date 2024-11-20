namespace API.Entitites;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }           // The question text
    public int CorrectOptionId { get; set; }    // The ID of the correct option
    public int QuizId { get; set; }             // Foreign key for the Quiz
    public int CategoryId { get; set; }         // Foreign key for the Category
}