using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    public static class WorkTimeLogBuilder
    {

        public static ModelBuilder WorkTimeLogBuild(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<WorkTimeLog>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.ProjectUser).WithMany(x => x.WorkTimeLogs)
                    .HasForeignKey(x => x.ProjectUserId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.WorkTask).WithMany(x => x.WorkTimeLogs)
                    .HasForeignKey(x => x.WorkTaskId).OnDelete(DeleteBehavior.NoAction);
                entity.ToTable("TaskManagementWorkTimeLog");


                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

            return modelBuilder;
        }

    }
        }
