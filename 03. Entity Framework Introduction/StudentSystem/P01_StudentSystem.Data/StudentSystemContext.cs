using Microsoft.EntityFrameworkCore;
using System;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {

        protected StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions options) 
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.CONNECTION_STRING);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
