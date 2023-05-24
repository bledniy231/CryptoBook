namespace WebAPITutorial.Models.Identity
{
	public record class UserRoles
	{
		public static string Student { get; } = "Student";
		public static string Teacher { get; } = "Teacher";
		public static string Admin { get; } = "Admin";
	}
}
