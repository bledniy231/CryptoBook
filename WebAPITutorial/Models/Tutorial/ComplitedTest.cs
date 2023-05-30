using Newtonsoft.Json;
using WebAPITutorial.Models.Identity;

namespace WebAPITutorial.Models.Tutorial
{
	[JsonObject(MemberSerialization.OptIn)]
	public class ComplitedTest
	{
		[JsonProperty]
		public int ComplitedTestId { get; set; }
		[JsonProperty]
		public DateTime ComplitedAt { get; set; }
		[JsonProperty]
		public double Mark { get; set; }
		[JsonProperty]
		public long UserId { get; set; }
		public virtual User User { get; set; } = null!;
		[JsonProperty]
		public int LessonId { get; set; }
		[JsonProperty]
		public virtual Lesson Lesson { get; set; } = null!;
		[JsonProperty]
		public virtual List<AnsweredQuestion> AnsweredQuestions { get; set; } = null!; 
	}
}
