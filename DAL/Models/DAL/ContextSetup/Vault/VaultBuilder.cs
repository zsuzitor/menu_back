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
            modelBuilder.Entity<Vault>().HasKey(x => x.Id);
            modelBuilder.Entity<Vault>().HasMany(x => x.Secrets).WithOne(x => x.Vault)
                .HasForeignKey(x => x.VaultId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Vault>().HasMany(x => x.Users).WithOne(x => x.Vault)
                .HasForeignKey(x => x.VaultId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(x => x.Vaults).WithOne(x => x.User)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vault>().ToTable("Vaults");
        }
    }
}
