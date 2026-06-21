using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    internal static class WorkTaskBuilder
    {
        public static ModelBuilder TaskManagementTaskBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkTask>(entity =>
            {
                entity.HasKey(x => x.Id);
                //entity.Property(e => e.IdString)
                //.IsRequired();

                // Создаем вычисляемую колонку
                entity.Property(e => e.IdString)
                    .HasComputedColumnSql("CAST([Id] AS NVARCHAR(20))", stored: true);

                // Создаем индекс для быстрого поиска
                entity.HasIndex(e => e.IdString);
                    //.HasDatabaseName("IX_YourEntity_YourLongColumnSearch");


                entity.HasMany(x => x.Comments).WithOne(x => x.Task)
                    .HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Status).WithMany(x => x.Tasks)
                    .HasForeignKey(x => x.StatusId).OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(x => x.Sprints).WithOne(x => x.Task)
                    .HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(x => x.Labels).WithOne(x => x.Task)
                    .HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.NoAction);

                entity.ToTable("TaskManagementTasks", schema: "TaskManagementApp");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты

            });

            return modelBuilder;

        }
    }
}
