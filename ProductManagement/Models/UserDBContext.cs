using Microsoft.EntityFrameworkCore;
using ProductManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Model
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options)
              : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>().HasKey(s => s.UserId);
            modelBuilder.Entity<Category>().HasKey(s => s.CategoryId);
            modelBuilder.Entity<Product>().HasKey(s => s.ProductId);

        }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }



    }
}
