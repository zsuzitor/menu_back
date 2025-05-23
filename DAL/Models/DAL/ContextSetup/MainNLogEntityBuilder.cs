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
            modelBuilder.Entity<MainNLogEntity>(entity => {
            entity.HasKey(x => x.Id).IsClustered(false);
                entity.Property(x => x.Id).HasDefaultValueSql("NEWID()");
                entity.HasIndex(x => x.EnteredDate);
                //https://docs.microsoft.com/ru-ru/ef/core/modeling/generated-properties?tabs=fluent-api
                entity.Property(x => x.EnteredDate).HasDefaultValueSql("GETDATE()");
                entity.ToTable("MainLogTable");
            });


        }
    }
}
