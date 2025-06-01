using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    internal static class ProjectUserBuilder
    {
        public static void ProjectUserBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectUser>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Comments).WithOne(x => x.Creator)
                    .HasForeignKey(x => x.CreatorId).OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(x => x.CreateByUser).WithOne(x => x.Creator)
                    .HasForeignKey(x => x.CreatorId).OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(x => x.ExecuteByUser).WithOne(x => x.Executor)
                    .HasForeignKey(x => x.ExecutorId).OnDelete(DeleteBehavior.NoAction);
                entity.ToTable("TaskManagementProjectUsers");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты


            });

        }
    }
}
