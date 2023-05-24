using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPITutorial.Models;

namespace WebAPITutorial.DBContexts
{
	public class UserContext : IdentityDbContext<UserModel, IdentityRole<long>, long>
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options) { }
	}
}
