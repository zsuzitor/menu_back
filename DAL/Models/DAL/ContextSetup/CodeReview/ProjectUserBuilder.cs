using BO.Models.CodeReviewApp.DAL.Domain;
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

            modelBuilder.Entity<ProjectUser>().HasMany(x => x.Comments).WithOne(x => x.Creator)
                .HasForeignKey(x => x.CreatorId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProjectUser>().HasMany(x => x.CreateByUser).WithOne(x => x.Creator)
                .HasForeignKey(x => x.CreatorId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProjectUser>().HasMany(x => x.ReviewByUser).WithOne(x => x.Reviewer)
                .HasForeignKey(x => x.ReviewerId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProjectUser>().ToTable("ReviewProjectUsers");
        }
    }
}
