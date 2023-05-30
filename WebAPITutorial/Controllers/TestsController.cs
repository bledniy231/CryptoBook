using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITutorial.DBContexts;
using WebAPITutorial.Models.Identity;
using WebAPITutorial.Models.Tutorial;

namespace WebAPITutorial.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class TestsController : ControllerBase
	{
		private readonly UserContext _userContext;
		private readonly UserManager<User> _userManager;

		public TestsController(UserContext userContext, UserManager<User> userManager)
		{
			_userContext = userContext;
			_userManager = userManager;
		}

		[Authorize]
		[HttpGet("{lessonId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<List<Question>>> GetRawTest(int lessonId)
		{
			List<Question> questions = await _userContext.Questions
				.IgnoreAutoIncludes()
				.Where(q => q.LessonId == lessonId).ToListAsync();
			if (questions.Count == 0) return NotFound($"Questions of lesson with {lessonId} not found");

			return Ok(questions);
		}

		[Authorize]
		[HttpGet("{username}/{complitedTestId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<ComplitedTest>> GetComplitedTest(string username, int complitedTestId)
		{
			User? user = await _userManager.FindByNameAsync(username);
			if (user == null) return NotFound($"User with username - \"{username}\" not found ");

			ComplitedTest? complitedTest = await _userContext.ComplitedTests
				.IgnoreAutoIncludes()
				.Include(compTest => compTest.AnsweredQuestions)
				.FirstOrDefaultAsync(compTest => compTest.ComplitedTestId == complitedTestId && compTest.UserId == user.Id);
			if (complitedTest == null) return NotFound($"\"{username}\"'s complited test with {complitedTestId} not found");

			return Ok(complitedTest);
		}

		[Authorize]
		[HttpGet("{username}")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<List<CompTestsResponse>>> GetAllComplitedTests(string username)
		{
			User? user = await _userManager.FindByNameAsync(username);
			if (user == null) return NotFound($"User with username - \"{username}\" not found ");

			List<CompTestsResponse> complitedTests = await _userContext.ComplitedTests
				.Include(compTest => compTest.Lesson)
				.Where(compTest => compTest.UserId == user.Id)
				.Select(compTest => new CompTestsResponse
				{
					ComplitedTestId = compTest.ComplitedTestId,
					ComplitedAt = compTest.ComplitedAt,
					LessonNumber = compTest.Lesson.LessonNumber,
					LessonTitle = compTest.Lesson.Title,
					Mark = compTest.Mark
				})
				.ToListAsync();
			if (complitedTests.Count == 0) return NotFound($"\"{username}\"'s complited tests not found");

			return Ok(complitedTests);
		}

		[Authorize]
		[HttpPost("{username}/{lessonId}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult> SaveResultsOfTest(string username, int lessonId, [FromBody] List<AnswerRequest> answerRequest)
		{
			User? user = await _userManager.FindByNameAsync(username);
			if (user == null) return NotFound($"User with username - \"{username}\" not found ");

			List<Question> questions = await _userContext.Questions.Where(q => q.LessonId == lessonId).ToListAsync();
			if (questions.Count == 0) return NotFound($"Questions with LessonID {lessonId} not found");

			ComplitedTest complitedTest = new ComplitedTest
			{
				ComplitedAt = DateTime.UtcNow,
				LessonId = lessonId,
				User = user
			};

			List<AnsweredQuestion> answeredQuestions = answerRequest
				.Select(ans => selection(ans, questions, user, complitedTest))
				.ToList();

			complitedTest.AnsweredQuestions = answeredQuestions;
			complitedTest.Mark = answeredQuestions.Sum(ans => ans.AccuracyOfAnswer);

			await _userContext.ComplitedTests.AddAsync(complitedTest);
			await _userContext.SaveChangesAsync();

			return CreatedAtAction(nameof(GetComplitedTest), 
				new 
				{ 
					complitedTestId = complitedTest.ComplitedTestId,
					username
				}, 
				complitedTest);
		}



		readonly Func<AnswerRequest, List<Question>, User, ComplitedTest, AnsweredQuestion> selection = 
		delegate (AnswerRequest ans, List<Question> questions, User user, ComplitedTest complitedTest)
		{
			double accuracy = 0;
			string[] rigthAnswers = questions.First(q => q.QuestionId == ans.QuestionId).RightAnswers.Split(';');
			string[] givenAnswers = Array.Empty<string>();
			if (ans.Answer != null)
				givenAnswers = ans.Answer.Split(';');

			accuracy = givenAnswers.Count(a => rigthAnswers.Contains(a)) / rigthAnswers.Length;

			return new AnsweredQuestion
			{
				QuestionId = ans.QuestionId,
				AccuracyOfAnswer = accuracy,
				GivenAnswer = ans.Answer,
				User = user,
				ComplitedTest = complitedTest
			};
		};
	}
}
