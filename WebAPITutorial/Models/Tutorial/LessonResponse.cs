namespace WebAPITutorial.Models.Tutorial
{
	public class LessonResponse
	{
		public int LessonId { get; set; }
		public int LessonNumber { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Text { get; set; } = null!;
		public List<string> ImageURLsStr { get; set; } = null!;
	}
}
