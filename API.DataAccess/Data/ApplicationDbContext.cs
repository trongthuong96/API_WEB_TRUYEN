using API.Models;
using API.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CategoryStory> CategoryStories { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Tick> Ticks { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Like>()
                .HasKey(l => new { l.StoryId, l.UserId });

            builder.Entity<Tick>()
                .HasKey(l => new { l.StoryId, l.UserId });

            builder.Entity<CategoryStory>()
                .HasKey(l => new { l.StoryId, l.CategoryId });

            /*builder
                .Entity<Like>()
                .HasOne(e => e.User)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);*/
        }
    }
}
