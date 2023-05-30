namespace WebAPITutorial.Models.Tutorial
{
	public class CompTestsResponse
	{
		public int ComplitedTestId { get; set; }
		public DateTime ComplitedAt { get; set; }
		public int LessonNumber { get; set; }
		public string LessonTitle { get; set; } = null!;
		public double Mark { get; set; }
	}
}
