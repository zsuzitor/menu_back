using BO.Models.VaultApp.Dal;
using BO.Models.WordsCardsApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.VaultApp
{
    internal static class VaultUserDalBuilder
    {
        public static void VaultUserDalBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VaultUserDal>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId);
                entity.HasIndex(x => x.VaultId);
                entity.HasOne(x => x.Vault).WithMany(x => x.Users)
                    .HasForeignKey(x => x.VaultId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
                entity.ToTable("VaultUsers");
            });

        }
    }
}
