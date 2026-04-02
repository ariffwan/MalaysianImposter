using Microsoft.EntityFrameworkCore;

namespace JanganKantoi.Models
{
	public class MyDbContext : DbContext
	{
		// put all database connection here
		public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
		{
		}


		// based on ERD diagram for any relationship between two tables
		//      protected override void OnModelCreating(ModelBuilder modelBuilder)
		//      {
		//	modelBuilder.Entity<Account>()
		//    .Property(p => p.accountid)
		//    .HasDefaultValueSql("gen_random_uuid()"); // PostgreSQL built-in function

		//	modelBuilder.Entity<BudgetPlan>()
		//    .HasMany(b => b.BudgetDetails)
		//    .WithOne()
		//    .HasForeignKey(d => d.budgetplanid)
		//    .OnDelete(DeleteBehavior.Cascade);
		//}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			//modelBuilder.Entity<Category>().ToTable("categories");
			//modelBuilder.Entity<Word>().ToTable("words");

			modelBuilder.Entity<Category>()
				.HasMany(c => c.Words)
				.WithOne(w => w.Category)
				.HasForeignKey(w => w.word_category)
				.OnDelete(DeleteBehavior.Cascade);
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Word> Words { get; set; }
	}
}
