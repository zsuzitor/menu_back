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

            modelBuilder.Entity<Secret>().HasKey(x => x.Id);
            modelBuilder.Entity<Secret>().HasIndex(x => x.VaultId);
            modelBuilder.Entity<Secret>().Property(x => x.Key).IsRequired();
            modelBuilder.Entity<Secret>().HasOne(x => x.Vault).WithMany(x => x.Secrets);

            modelBuilder.Entity<Secret>().ToTable("Secrets");
        }
    }
}
