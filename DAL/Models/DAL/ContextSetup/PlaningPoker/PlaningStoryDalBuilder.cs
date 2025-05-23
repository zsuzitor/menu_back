using BO.Models.PlaningPoker.DAL;
using BO.Models.VaultApp.Dal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.PlaningPoker
{
    internal static class PlaningStoryDalBuilder
    {
        public static void PlaningStoryDalBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaningStoryDal>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Room).WithMany(x => x.Stories)
                    .HasForeignKey(x => x.RoomId);
                entity.Property(e => e.Vote)
                    .HasColumnType("decimal(18,4)");
                entity.ToTable("PlaningStories");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });


        }
    }
}
