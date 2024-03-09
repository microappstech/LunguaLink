using Langua.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.DataContext.Data
{
    public class LanguaContext : DbContext
    {

        public LanguaContext(DbContextOptions<LanguaContext> dbContextOptions):base(dbContextOptions)
        {            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Department>()
                .HasOne(i => i.Manager)
                .WithOne(i => i.Department)
                .HasForeignKey<Manager>(i => i.DepartmentId);

            
        }
        public DbSet<Candidat> Candidates { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<MessageGroup> MessageGroups { get; set; }
        public DbSet<MessageUser> MessageUsers { get; set; }
    }
}
