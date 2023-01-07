using Creative_blog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Creative_blog.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<Service> Services { get; set; }
        public DbSet<OurService> OurServices { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Stat> Stats { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<WeSee> WeSees { get; set; }
        public DbSet<WhatWeDo> WhatWeDos { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Service>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<OurService>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Blog>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Cast>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<ContactInfo>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Portfolio>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Stat>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Testimonial>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<WeSee>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<WhatWeDo>().HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
