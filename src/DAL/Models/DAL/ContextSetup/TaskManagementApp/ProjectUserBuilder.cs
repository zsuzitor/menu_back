using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    internal static class ProjectUserBuilder
    {
        public static ModelBuilder ProjectUserBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectUser>(entity =>
            {
                entity.ToTable("TaskManagementProjectUsers", schema: "TaskManagementApp");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты


            });

            return modelBuilder;
        }
    }
}
