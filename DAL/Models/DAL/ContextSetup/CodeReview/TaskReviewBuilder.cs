using BO.Models.CodeReviewApp.DAL.Domain;
using BO.Models.MenuApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.CodeReview
{
    internal static class TaskReviewBuilder
    {
        public static void TaskReviewBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskReview>(entity => {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Comments).WithOne(x => x.Task)
                    .HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Status).WithMany(x => x.Tasks)
                    .HasForeignKey(x => x.StatusId).OnDelete(DeleteBehavior.SetNull);

                entity.ToTable("ReviewTasks");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты

            });


        }
    }
}
