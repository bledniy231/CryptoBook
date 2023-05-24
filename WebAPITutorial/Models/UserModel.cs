using Microsoft.AspNetCore.Identity;

namespace WebAPITutorial.Models
{
	public class UserModel : IdentityUser<long>
	{
		public string FirstName { get; set; } = null!;
		public string? SecondName { get; set; }
		public string? ImageLink { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiryTime { get; set; }
	}
}
