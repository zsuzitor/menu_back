using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.CodeReview
{
    internal static class ProjectUserBuilder
    {
        public static void ProjectUserBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectUser>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Comments).WithOne(x => x.Creator)
                    .HasForeignKey(x => x.CreatorId).OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(x => x.CreateByUser).WithOne(x => x.Creator)
                    .HasForeignKey(x => x.CreatorId).OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(x => x.ReviewByUser).WithOne(x => x.Reviewer)
                    .HasForeignKey(x => x.ReviewerId).OnDelete(DeleteBehavior.NoAction);
                entity.ToTable("ReviewProjectUsers");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты


            });

        }
    }
}
