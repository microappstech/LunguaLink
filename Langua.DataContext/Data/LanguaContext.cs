using Langua.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
    public class LanguaContext(DbContextOptions<Langua.DataContext.Data.LanguaContext> options) : IdentityDbContext<ApplicationUser>(options)
    //public class LanguaContext : DbContext
    {

        //public LanguaContext(DbContextOptions<LanguaContext> dbContextOptions):base(dbContextOptions)
        //{            
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("UserRoleClaim");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");


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
                .HasOne(i => i.Departement)
                .WithMany(i => i.Teachers)
                .HasForeignKey(i => i.DepartementId)
                .HasPrincipalKey(i => i.Id);

            builder.Entity<Candidat>()
                .HasOne(i => i.Departement)
                .WithMany(i => i.Candidates)
                .HasForeignKey(i => i.DepartementId)
                .HasPrincipalKey(i => i.Id);


            builder.Entity<Teacher>()
                .ToTable(tb => tb.UseSqlOutputClause());
            builder.Entity<Teacher>()
                .ToTable(tb => tb.HasTrigger("delete_user_on_teacher_deleted"));
            builder.Entity<Candidat>()
                .ToTable(tb => tb.HasTrigger("delete_user_on_candidate_deleted"));
            builder.Entity<Manager>().ToTable(tm => tm.HasTrigger("delete_user_on_manager_deleted"));

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<Candidat>()
            .HasOne(c => c.User)
            .WithOne(u => u.Candidate)
            .HasForeignKey<Candidat>(c => c.UserId);


            builder.Entity<Teacher>()
            .HasOne(c => c.User)
            .WithOne(u => u.Teacher)
            .HasForeignKey<Teacher>(c => c.UserId);

            builder.Entity<MessageGroup>()
                .HasOne(mg => mg.User)
                .WithMany(u => u.MessagesGroup)
                .HasForeignKey(gm => gm.SenderId)
                .HasPrincipalKey(i => i.Id);

            builder.Entity<Ressource>().
                HasOne(i => i.Teacher)
                .WithMany(i => i.Ressources)
                .HasForeignKey(i => i.TeacherId)
                .HasPrincipalKey(i => i.Id);

            builder.Entity<ContentGroup>()
                .HasOne(i => i.Group)
                .WithMany(i => i.GroupRessources)
                .HasForeignKey(i => i.GroupId)
                .HasPrincipalKey(i => i.Id);
            builder.Entity<ContentGroup>()
                .HasOne(i => i.Ressource)
                .WithMany(i => i.GroupRessources)
                .HasForeignKey(i => i.RessourceId)
                .HasPrincipalKey(i => i.Id);
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
        public DbSet<Ressource> Ressources { get; set; }
        public DbSet<ContentGroup> GroupRessources { get; set; }
    }
}
