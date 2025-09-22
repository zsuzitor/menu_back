using BO.Models.VaultApp.Dal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.VaultApp
{
    internal static class SecretBuilder
    {
        public static void SecretBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Secret>(entity => {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.VaultId);
                entity.Property(x => x.Key).IsRequired();
                entity.HasOne(x => x.Vault).WithMany(x => x.Secrets);
                entity.ToTable("Secrets");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

            
        }
    }
}
