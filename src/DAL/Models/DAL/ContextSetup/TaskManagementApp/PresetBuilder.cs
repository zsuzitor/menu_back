using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    public static class PresetBuilder
    { 
        public static ModelBuilder TaskManagementPresetBuild(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Preset>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Status).WithMany()
                .HasForeignKey(x => x.StatusId).OnDelete(DeleteBehavior.NoAction);
            entity.HasMany(x => x.Labels).WithOne(x => x.Preset)
                .HasForeignKey(x => x.PresetId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.Project).WithMany()
                .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.Creator).WithMany()
                .HasForeignKey(x => x.CreatorId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.Executor).WithMany()
                .HasForeignKey(x => x.ExecutorId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.Sprint).WithMany()
                .HasForeignKey(x => x.SprintId).OnDelete(DeleteBehavior.NoAction);

            entity.ToTable("TaskManagementPreset");

            entity.Property(p => p.RowVersion)
                .IsRowVersion() // Автоматически обновляется SQL Server
                .IsConcurrencyToken(); // Включает проверку на конфликты

        });

        return modelBuilder;

    }
}
}
