using LifeMaker.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using LifeMaker.Models.ReferencePrices;
using LifeMaker.Models.MoneySpendings;

namespace LifeMaker.DataAccess
{
    public class ReferencePricesDB : DbContext
    {
        public DbSet<ReferencePrices> Reference_Prices { get; set; }
        public DbSet<ReferencePricesStore> Reference_Prices_Store { get; set; }
        public DbSet<ReferencePricesCategory> Reference_Prices_Category { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=lifemaker;user=root;password=44f91C29ec");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReferencePrices>().Ignore(i => i.CategoryDDL);
            modelBuilder.Entity<ReferencePrices>().Ignore(i => i.StoreDDL);

            modelBuilder.Entity<ReferencePrices>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.CategoryId).IsRequired(false);
                entity.Property(e => e.StoreId).IsRequired(false);
            });

            modelBuilder.Entity<ReferencePricesCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ReferencePricesStore>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Address).IsRequired(false);
            });
        }
    }
}
