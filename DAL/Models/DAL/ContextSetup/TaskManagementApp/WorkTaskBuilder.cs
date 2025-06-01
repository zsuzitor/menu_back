using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    internal static class WorkTaskBuilder
    {
        public static void TaskManagementTaskBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkTask>(entity => {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Comments).WithOne(x => x.Task)
                    .HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Status).WithMany(x => x.Tasks)
                    .HasForeignKey(x => x.StatusId).OnDelete(DeleteBehavior.NoAction);

                entity.ToTable("TaskManagementTasks");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты

            });


        }
    }
}
