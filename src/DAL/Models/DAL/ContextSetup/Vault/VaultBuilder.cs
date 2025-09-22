using BO.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

using BO.Models.VaultApp.Dal;

namespace DAL.Models.DAL.ContextSetup.VaultApp
{
    internal static class VaultBuilder
    {
        public static void VaultBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vault>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.Secrets).WithOne(x => x.Vault)
                    .HasForeignKey(x => x.VaultId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.Users).WithOne(x => x.Vault)
                    .HasForeignKey(x => x.VaultId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
                entity.ToTable("Vaults");
            });

            modelBuilder.Entity<User>().HasMany(x => x.Vaults).WithOne(x => x.User)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
