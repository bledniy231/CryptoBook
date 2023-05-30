using Microsoft.AspNetCore.Identity;
using System.Net;
using WebAPITutorial.Models.Tutorial;

namespace WebAPITutorial.Models.Identity
{
    public class User : IdentityUser<long>
    {
        public string FirstName { get; set; } = null!;
        public string? SecondName { get; set; }
        public string? Description { get; set; }
        public string? ImageLink { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public virtual List<ComplitedTest> ComplitedTests { get; set; } = null!;
    }
}
