using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    public static class ProjectSprintBuilder
    {
        public static ModelBuilder ProjectSprintBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectSprint>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementSprint");

                entity.HasMany(x => x.Tasks).WithOne(x => x.Sprint)
                    .HasForeignKey(x => x.SprintId).OnDelete(DeleteBehavior.NoAction);

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

            return modelBuilder;
        }
    }
}
