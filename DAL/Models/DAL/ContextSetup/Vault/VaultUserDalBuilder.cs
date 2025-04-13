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

            modelBuilder.Entity<VaultUserDal>().HasKey(x => x.Id);
            modelBuilder.Entity<VaultUserDal>().HasIndex(x => x.UserId);
            modelBuilder.Entity<VaultUserDal>().HasIndex(x => x.VaultId);
            modelBuilder.Entity<VaultUserDal>().HasOne(x => x.Vault).WithMany(x => x.Users)
                .HasForeignKey(x => x.VaultId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VaultUserDal>().ToTable("VaultUsers");
        }
    }
}
