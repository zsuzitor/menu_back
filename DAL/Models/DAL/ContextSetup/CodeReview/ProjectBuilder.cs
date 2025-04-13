using BO.Models.CodeReviewApp.DAL.Domain;
using BO.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.CodeReview
{
    internal static class ProjectBuilder
    {
        public static void ProjectBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasKey(x => x.Id);
            modelBuilder.Entity<Project>().HasMany(x => x.Tasks).WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Project>().HasMany(x => x.Users).WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasMany(x => x.CodeReviewProjects).WithOne(x => x.MainAppUser)
                .HasForeignKey(x => x.MainAppUserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Project>().ToTable("ReviewProject");
        }
    }
}
