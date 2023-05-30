namespace WebAPITutorial.Models.Tutorial
{
	public class Lesson
	{
		public int LessonId { get; set; }
		public int LessonNumber { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Text { get; set; } = null!;
		public virtual List<ImageURL> ImageURLs { get; set; } = null!;
		public virtual List<Question> Questions { get; set; } = null!;
	}
}
