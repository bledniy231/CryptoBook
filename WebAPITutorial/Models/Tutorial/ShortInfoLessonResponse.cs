namespace WebAPITutorial.Models.Tutorial
{
	public class ShortInfoLessonResponse
	{
		public int LessonId { get; set; }
		public int LessonNumber { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
	}
}
