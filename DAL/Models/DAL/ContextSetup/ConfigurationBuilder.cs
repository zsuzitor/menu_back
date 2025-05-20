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

            modelBuilder.Entity<Configuration>().HasKey(x => x.Id);

            modelBuilder.Entity<Configuration>().ToTable("Configurations");
        }
    }
}
