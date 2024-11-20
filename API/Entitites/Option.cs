namespace API.Entitites;

public class Option
{
    public int Id { get; set; }
    public string Text { get; set; }            // Option text
    public int QuestionId { get; set; }         // Foreign key to the Question
}