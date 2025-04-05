using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationMember,IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Shift> Shifts { get; set; }

        public DbSet<Project> Projects { get; set; }    

        public DbSet<ProjectMember> ProjectMembers { get; set; }

        public DbSet<ProjectItem> ProjectItems { get; set; }
        public DbSet<ProjectItemMember> ProjectItemAssigns { get; set; }

        public DbSet<Rule> Rules { get; set; }
        public DbSet<RuleViolation> MemberRuleViolations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUserLogin<string>>()
                .HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });

            builder.Entity<IdentityUserRole<string>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.Entity<IdentityUserClaim<string>>()
                .HasKey(uc => uc.Id);

            builder.Entity<IdentityUserToken<string>>()
                .HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });



        }
       
    }
}
