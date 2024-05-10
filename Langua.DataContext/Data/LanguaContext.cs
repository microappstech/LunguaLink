using Langua.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
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

            builder.Entity<Groups>()
                .HasMany(i=>i.Candidats)
                .WithOne(i=>i.Group)
                .HasForeignKey(i=>i.GroupId)
                .HasPrincipalKey(i=>i.Id)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Teacher>()
                .ToTable(tb => tb.UseSqlOutputClause());
            builder.Entity<Teacher>()
                .ToTable(tb => tb.HasTrigger("delete_user_on_teacher_deleted"));
            builder.Entity<Candidat>()
                .ToTable(tb => tb.HasTrigger("delete_user_on_candidate_deleted"));




            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<Candidat>()
        .HasOne(c => c.User)
        .WithOne(u => u.Candidate)
        .HasForeignKey<Candidat>(c => c.UserId);


        }



        public DbSet<Candidat> Candidates { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<MessageGroup> MessageGroups { get; set; }
        public DbSet<MessageUser> MessageUsers { get; set; }
        public DbSet<GroupCandidates> GroupCandidates { get; set; }
        public DbSet<GroupTeacher> GroupTeachers { get; set; }
        public DbSet<Session> Sessions { get; set; }
    }
}
