using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    public static class WorkTaskSprintRelationBuilder
    {
        public static ModelBuilder WorkTaskSprintRelationBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkTaskSprintRelation>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementSprintTaskRelation");

            });

            return modelBuilder;
        }
    }
}
