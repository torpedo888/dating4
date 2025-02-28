using Microsoft.AspNetCore.Mvc;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Entitites;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    [HttpPost("create-quiz")]
    public async Task<IActionResult> PostQuiz([FromBody] Quiz quiz)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _context.Quizzes.AddAsync(quiz);
        await _context.SaveChangesAsync(); // This will set quiz.Id automatically

        return Ok(quiz);
    }

    [HttpPost("create-category")]
    public async Task<IActionResult> PostCategory([FromBody] Category category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync(); // This will set category.Id automatically

        return Ok(category);
    }


    // POST: api/questions/create
    [HttpPost("create")]
    public async Task<IActionResult> PostQuestion([FromBody] QuestionCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Create a new Question entity
        var question = new Question
        {
            Text = request.Text,
            CorrectOptionId = request.CorrectOptionId,
            QuizId = request.QuizId,
            CategoryId = request.CategoryId
        };

        // Add the question to the context
        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();

        // Get the ID of the newly created question
        var questionId = question.Id;

        // Create the options and set the QuestionId
        foreach (var option in request.Options)
        {
            var newOption = new Option
            {
                Text = option.Text,
                QuestionId = questionId
            };

            await _context.Options.AddAsync(newOption);
        }

        // Save all changes to the database
        await _context.SaveChangesAsync();

        return Ok(question);
    }


    // Example of another POST method, e.g., for batch creation or another action
    // [HttpPost("bulk-create")] // Differentiate this with a unique route
    // public IActionResult PostMultipleQuestions([FromBody] List<Question> questions)
    // {
    //     // Implementation for adding multiple questions
    //     // Similar validation and save logic
    // }

    // GET method for retrieving a question
    [HttpGet("{id}")] // Example of a GET method
    public IActionResult GetQuestion(int id)
    {
        var question = _context.Questions.Find(id);
        if (question == null)
        {
            return NotFound();
        }
        return Ok(question);
    }

    [HttpGet]
    public async Task<IActionResult> GetQuestions()
    {
        try
        {
            // Get all questions
            var questions = await _context.Questions.ToListAsync();

            // Get all options related to those questions
            var options = await _context.Options.ToListAsync();

            // Group options by QuestionId
            var questionsWithOptions = questions.Select(q => new
            {
                Question = q,
                Options = options.Where(o => o.QuestionId == q.Id).ToList() // Get options for the current question
            }).ToList();

            return Ok(questionsWithOptions);
        }
        catch (Exception ex)
        {
            // Handle exception, log it if necessary
            return StatusCode(500, ex.Message);
        }
    }
}

public class QuestionCreateRequest
{
    public string Text { get; set; }
    public int CorrectOptionId { get; set; }
    public List<OptionCreateRequest> Options { get; set; } = [];
    public int QuizId { get; set; }
    public int CategoryId { get; set; }
}

public class OptionCreateRequest
{
    public string Text { get; set; }
}
