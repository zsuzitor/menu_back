using BO.Models.DAL.Domain;
using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.CodeReview
{
    internal static class ProjectBuilder
    {
        public static void ProjectBuild(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Project>(entity => {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Tasks).WithOne(x => x.Project)
                    .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.Users).WithOne(x => x.Project)
                    .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.TaskStatuses).WithOne(x => x.Project)
                    .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
                entity.ToTable("ReviewProject");

            });


            modelBuilder.Entity<User>().HasMany(x => x.CodeReviewProjects).WithOne(x => x.MainAppUser)
                .HasForeignKey(x => x.MainAppUserId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
