namespace WebAPITutorial.Models.Identity
{
	public class RegisterResponse
	{
		public string Username { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
