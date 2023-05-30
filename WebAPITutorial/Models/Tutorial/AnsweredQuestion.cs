using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebAPITutorial.Models.Identity;

namespace WebAPITutorial.Models.Tutorial
{
	[JsonObject(MemberSerialization.OptIn)]
	public class AnsweredQuestion
	{
		[JsonProperty]
		public int AnsweredQuestionId { get; set; }
		public string? GivenAnswer { get; set; }
		[NotMapped]
		[JsonProperty]
		public List<string> GivenAnswerInList => GivenAnswer?.Split(';').ToList() ?? new List<string>();
		[JsonProperty]
		public double AccuracyOfAnswer { get; set; }
		[JsonProperty]
		public int QuestionId { get; set; }
		[JsonProperty]
		public virtual Question Question { get; set; } = null!;
		public int ComplitedTestId { get; set; }
		public virtual ComplitedTest ComplitedTest { get; set; } = null!;
		public long UserId { get; set; }
		public virtual User User { get; set; } = null!;
	}
}
