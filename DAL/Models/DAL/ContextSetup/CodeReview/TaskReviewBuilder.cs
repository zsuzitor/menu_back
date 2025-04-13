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
            modelBuilder.Entity<TaskReview>().HasMany(x => x.Comments).WithOne(x => x.Task)
                .HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TaskReview>().ToTable("ReviewTasks");

        }
    }
}
