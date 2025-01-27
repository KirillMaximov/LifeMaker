using LifeMaker.Models.KnowledgeBase;
using LifeMaker.Models.NiceTime;
using LifeMaker.Models.Notes;
using Microsoft.EntityFrameworkCore;

namespace LifeMaker.DataAccess
{
    public class NiceTimeDB : DbContext
    {
        public DbSet<NiceTime> Nice_time { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=lifemaker;user=root;password=44f91C29ec");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NiceTime>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Link).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired(false);
                entity.Property(e => e.IsActive).IsRequired(false);
                entity.Property(e => e.OrderBy).IsRequired(false);
            });
        }
    }
}
