using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BloggingApp.Models;

namespace BloggingApp.Data 
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Blog> Blog { get; set; } = default!;
        public DbSet<BlogTag> BlogTag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             // Configure one-to-many relationship
            modelBuilder.Entity<Blog>()
            .HasMany(b => b.Tags)
            .WithOne(t => t.Blog)
            .HasForeignKey(t => t.BlogId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
