using LifeMaker.Models.Notes;
using Microsoft.EntityFrameworkCore;

namespace LifeMaker.DataAccess
{
    public class NotesDB : DbContext
    {
        public DbSet<Notes> Notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=lifemaker;user=root;password=44f91C29ec");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notes>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Text).IsRequired();
            });
        }
    }
}
