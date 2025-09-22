using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    internal static class WorkTaskCommentBuilder
    {

        public static ModelBuilder WorkTaskCommentBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkTaskComment>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementTaskComment");


                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

            return modelBuilder;
        }
    }
}
