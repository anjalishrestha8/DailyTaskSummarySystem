using ClientWebApi.Models.Entities;
using ClientWebApi.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClientWebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<UserTaskSummary> UserTaskSummary { get; set; }
        public DbSet<Comments> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Comments>()
                .HasOne(c => c.UserTaskSummary)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserTaskSummaryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}