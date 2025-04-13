using BO.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup
{
    internal static class MainNLogEntityBuilder
    {
        public static void MainNLogEntityBuild(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MainNLogEntity>().HasKey(x => x.Id).IsClustered(false);
            modelBuilder.Entity<MainNLogEntity>().Property(x => x.Id).HasDefaultValueSql("NEWID()");


            modelBuilder.Entity<MainNLogEntity>().HasIndex(x => x.EnteredDate);
            //https://docs.microsoft.com/ru-ru/ef/core/modeling/generated-properties?tabs=fluent-api
            modelBuilder.Entity<MainNLogEntity>().Property(x => x.EnteredDate).HasDefaultValueSql("GETDATE()");
            //modelBuilder.Entity<MainNLogEntity>().Property(x => x.EnteredDate).ValueGeneratedOnAdd();
            //    //.HasDefaultValue(3);
            //    .HasDefaultValueSql("getdate()");
            //    //.ValueGeneratedOnAdd
            modelBuilder.Entity<MainNLogEntity>().ToTable("MainLogTable");
        }
    }
}
