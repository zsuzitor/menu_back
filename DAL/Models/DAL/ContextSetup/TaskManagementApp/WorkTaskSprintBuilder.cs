using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    public static class ProjectSprintBuilder
    {
        public static void ProjectSprintBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectSprint>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementSprint");


                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

        }
    }
}
