using BO.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
