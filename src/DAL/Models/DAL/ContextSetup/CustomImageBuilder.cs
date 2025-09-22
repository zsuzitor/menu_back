using BO.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup
{
    internal static class CustomImageBuilder
    {
        public static void CustomImageBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomImage>(entity =>
            {
                entity.HasKey(x => x.Id);


                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
                entity.ToTable("Images");
            });

        }
    }
}
