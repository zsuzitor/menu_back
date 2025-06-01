using BO.Models.TaskManagementApp.DAL.Domain;
using BO.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.CodeReview
{
    public static class TaskReviewStatusBuilder
    {
        public static void TaskStatusBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskReviewStatus>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskReviewStatus");


                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

        }
    }
}
