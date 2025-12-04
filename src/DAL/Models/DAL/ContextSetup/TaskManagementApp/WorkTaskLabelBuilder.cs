using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    internal static class WorkTaskLabelBuilder
    {
        public static ModelBuilder WorkTaskLabelBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkTaskLabel>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementLabel");


                entity.HasMany(x => x.Tasks).WithOne(x => x.Label)
                    .HasForeignKey(x => x.LabelId).OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(x => x.Preset).WithOne(x => x.Label)
                    .HasForeignKey(x => x.LabelId).OnDelete(DeleteBehavior.NoAction);

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

            return modelBuilder;
        }
    }
}
