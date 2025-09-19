using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    public static class WorkTaskLabelTaskBuilder
    {
        public static void WorkTaskLabelTaskBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkTaskLabelTask>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementLabelTask");

            });

        }
    }
}
