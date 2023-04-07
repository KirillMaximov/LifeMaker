using LifeMaker.Models.MoneySpendings;
using Microsoft.EntityFrameworkCore;

namespace LifeMaker.DataAccess
{
    public class MoneySpendingsDB : DbContext
    {
        public DbSet<MoneySpendings> Money_Spendings { get; set; }
        public DbSet<MoneySpendingsPerson> Money_Spendings_Person { get; set; }
        public DbSet<MoneySpendingsCategory> Money_Spendings_Category { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=lifemaker;user=root;password=44f91C29ec");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MoneySpendings>().Ignore(i => i.CategoryDDL);
            modelBuilder.Entity<MoneySpendings>().Ignore(i => i.PersonDDL);

            modelBuilder.Entity<MoneySpendings>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).IsRequired();
            });

            modelBuilder.Entity<MoneySpendingsCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<MoneySpendingsPerson>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });
        }
    }
}
