using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    internal static class WorkTaskCommentBuilder
    {

        public static ModelBuilder WorkTaskCommentBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkTaskStatus>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementComment");


                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

            return modelBuilder;
        }
    }
}
