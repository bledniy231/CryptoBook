using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITutorial.Models.Tutorial
{
	public class Question
	{
		public int QuestionId { get; set; }
		public string QuestionText { get; set; } = null!;
		[JsonIgnore]
		public string PossibleAnswers { get; set; } = null!;
		[NotMapped]
		public List<string> PossibleAnswersInList => PossibleAnswers.Split(';').ToList();
		[JsonIgnore]
		public string RightAnswers { get; set; } = null!;
		[NotMapped]
		public List<string> RightAnswersInList => RightAnswers.Split(';').ToList();
		public bool IsStringAnswer { get; set; }
		public int LessonId { get; set; }
		[JsonIgnore]
		public virtual Lesson Lesson { get; set; } = null!;
	}
}
