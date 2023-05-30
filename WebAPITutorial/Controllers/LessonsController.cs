using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Net.Http.Headers;
using WebAPITutorial.DBContexts;
using WebAPITutorial.Models.Identity;
using WebAPITutorial.Models.Tutorial;

namespace WebAPITutorial.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class LessonsController : ControllerBase
	{
		private readonly UserContext _userContext;

		public LessonsController(UserContext userContext) 
		{
			_userContext = userContext;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<List<ShortInfoLessonResponse>>> GetAllLessons()
		{
			return Ok(await _userContext.Lessons.Select(lesson => new ShortInfoLessonResponse
			{
				LessonId = lesson.LessonId,
				LessonNumber = lesson.LessonNumber,
				Title = lesson.Title,
				Description = lesson.Description
			}).ToListAsync());
		}

		[Authorize]
		[HttpGet("{lessonId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<LessonResponse>> GetLesson(int lessonId)
		{
			Lesson? lesson = await _userContext.Lessons
				.Include(l => l.ImageURLs)
				.FirstOrDefaultAsync(l => l.LessonId == lessonId);
			if (lesson == null) return NotFound($"Lesson with {lessonId} not found");


			LessonResponse lessonResponse = new LessonResponse
			{
				LessonId = lesson.LessonId,
				LessonNumber = lesson.LessonNumber,
				Title = lesson.Title,
				Description = lesson.Description,
				Text = lesson.Text,
				ImageURLsStr = lesson.ImageURLs.Select(i => i.URL).ToList()
			};

			return Ok(lessonResponse);
		}


		[Authorize]
		[HttpGet("{lessonId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetLessonImages(int lessonId)
		{
			Lesson? lesson = await _userContext.Lessons.FindAsync(lessonId);
			if (lesson == null) return NotFound($"Lesson with {lessonId} not found");

			var multipartContent = new MultipartContent();

			foreach (ImageURL imageURL in lesson.ImageURLs)
			{
				var conDispVal = new ContentDispositionHeaderValue("attachment")
				{
					FileName = Path.GetFileName(imageURL.URL)
				};

				byte[] bytesCurrentImage = await System.IO.File.ReadAllBytesAsync(imageURL.URL);
				var memStream = new MemoryStream(bytesCurrentImage);
				var imageContent = new ByteArrayContent(memStream.ToArray());

				imageContent.Headers.ContentDisposition = conDispVal;
				imageContent.Headers.ContentType = new MediaTypeHeaderValue(MimeTypes.GetMimeType(Path.GetFileName(imageURL.URL)));
				multipartContent.Add(imageContent);
			}

			return Ok(new HttpResponseMessage { Content = multipartContent });
		}
	}
}
