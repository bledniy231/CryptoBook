using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPITutorial.Models.Identity;
using WebAPITutorial.Models.Tutorial;

namespace WebAPITutorial.DBContexts
{
    public class UserContext : IdentityDbContext<User, IdentityRole<long>, long>
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options) { }

		public DbSet<Lesson> Lessons { get; set; }
		public DbSet<ImageURL> ImageURLs { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<ComplitedTest> ComplitedTests { get; set; }
		public DbSet<AnsweredQuestion> AnsweredQuestions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasMany(u => u.ComplitedTests)
				.WithOne(t => t.User)
				.HasForeignKey(t => t.UserId);

			modelBuilder.Entity<ImageURL>()
				.ToTable("ImageURLs")
				.HasOne(i => i.Lesson)
				.WithMany(l => l.ImageURLs)
				.HasForeignKey(i => i.LessonId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Question>()
				.ToTable("Questions")
				.HasOne(q => q.Lesson)
				.WithMany(l => l.Questions)
				.HasForeignKey(q => q.LessonId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<ComplitedTest>()
				.ToTable("ComplitedTests")
				.HasMany(t => t.AnsweredQuestions)
				.WithOne(a => a.ComplitedTest)
				.HasForeignKey(a => a.ComplitedTestId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Lesson>()
				.ToTable("Lessons");

			base.OnModelCreating(modelBuilder);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}
	}
}
