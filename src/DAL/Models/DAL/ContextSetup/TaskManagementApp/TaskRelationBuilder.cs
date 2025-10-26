using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    public static class TaskRelationBuilder
    {
        public static ModelBuilder TaskRelatioBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskRelation>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementTaskRelation");

                entity.HasOne(x => x.MainWorkTask)
                //.WithMany()
                .WithMany(x => x.MainWorkTasksRelation)
                .HasForeignKey(x => x.MainWorkTaskId).OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.SubWorkTask)
                //.WithMany()
                .WithMany(x => x.SubWorkTasksRelation)
                .HasForeignKey(x => x.SubWorkTaskId).OnDelete(DeleteBehavior.NoAction);

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

            return modelBuilder;
        }
    }
}
