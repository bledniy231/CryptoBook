using Newtonsoft.Json;

namespace WebAPITutorial.Models.Tutorial
{
	[JsonObject(MemberSerialization.OptIn)]
	public class ImageURL
	{
		[JsonProperty]
		public int ImageURLId { get; set; }
		[JsonProperty]
		public string URL { get; set; } = null!;
		[JsonProperty]
		public int LessonId { get; set; }
		public virtual Lesson Lesson { get; set; } = null!;
	}
}
