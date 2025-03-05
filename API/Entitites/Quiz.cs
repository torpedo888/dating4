using System.Text.Json.Serialization;

namespace API.Entitites;

public class Quiz
{
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    // Foreign key for Category
    public int CategoryId { get; set; }
    public Category Category { get; set; }  // Navigation property

     // Navigation property to Questions
    public ICollection<Question> Questions { get; set; } = new List<Question>();  // Navigation property to Questions
}