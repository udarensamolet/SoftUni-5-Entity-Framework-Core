using Blog.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Data.Context
{
    public class BlogContext : IdentityDbContext
    {
        public BlogContext() 
        { 
        }

        public BlogContext(DbContextOptions<BlogContext> options)
            : base(options) 
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.UserName)
                .HasMaxLength(20)
            .IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.Email)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder
                .Entity<Category>()
                .HasData(new Category()
                {
                    Id = 1,
                    Name = "Fun"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Gossip"
                },
                new Category()
                {
                    Id = 3,
                    Name = "News"
                },
                new Category()
                {
                    Id = 4,
                    Name = "Sports"
                },
                new Category()
                {
                    Id = 5,
                    Name = "Useful"
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
