using Fundoo.Notes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fundoo.Notes.Infrastructure.Data
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.UserId).IsRequired();
            });
        }
    }
}