using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    public static class WorkTaskStatusBuilder
    {
        public static ModelBuilder TaskStatusBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkTaskStatus>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementTaskStatus");


                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

            return modelBuilder;
        }
    }
}
