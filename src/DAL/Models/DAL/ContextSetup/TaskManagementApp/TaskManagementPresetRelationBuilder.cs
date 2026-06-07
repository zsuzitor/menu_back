using BO.Models.TaskManagementApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.TaskManagementApp
{
    internal static class TaskManagementPresetRelationBuilder
    {
        public static ModelBuilder TaskManagementPresetRelationBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkTaskLabelPresetRelation>(entity => {
                entity.HasKey(x => x.Id);
                entity.ToTable("TaskManagementPresetRelation", schema: "TaskManagementApp");
            });

            return modelBuilder;
        }
    }
}
