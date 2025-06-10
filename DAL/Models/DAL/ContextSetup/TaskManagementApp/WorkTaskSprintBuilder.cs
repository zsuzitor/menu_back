using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    public static class WorkTaskSprintBuilder
    {
        public static void WorkTaskSprintBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkTaskSprint>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementSprint");


                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

        }
    }
}
