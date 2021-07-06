using Microsoft.EntityFrameworkCore;
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
            //config primary key(User)
            modelBuilder.Entity<UserInfo>().HasKey(s => s.UserId);

        }
        public DbSet<UserInfo> UserInfo{ get; set; }


    }
}
