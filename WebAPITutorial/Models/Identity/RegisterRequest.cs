namespace WebAPITutorial.Models.Identity
{
	public class RegisterRequest
	{
		public string Username { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string PasswordConfirm { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string? SecondName { get; set; }
	}
}
