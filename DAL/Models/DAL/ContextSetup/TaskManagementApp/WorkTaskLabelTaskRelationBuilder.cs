using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    public static class WorkTaskLabelTaskRelationBuilder
    {
        public static ModelBuilder WorkTaskLabelTaskRelationBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkTaskLabelTaskRelation>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementLabelTaskRelation");

            });

            return modelBuilder;
        }
    }
}
