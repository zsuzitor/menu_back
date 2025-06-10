using BO.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup
{
    internal static class ConfigurationBuilder
    {
        public static void ConfigurationBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Key).IsUnique();
                entity.ToTable("Configurations");

            });
        }
    }
}
