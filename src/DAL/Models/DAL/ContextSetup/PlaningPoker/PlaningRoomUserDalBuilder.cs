using BO.Models.PlaningPoker.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.PlaningPoker
{
    internal static class PlaningRoomUserDalBuilder
    {
        public static void PlaningRoomUserDalBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaningRoomUserDal>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.MainAppUserId).IsRequired();
                entity.HasOne(x => x.MainAppUser).WithMany()
                    .HasForeignKey(x => x.MainAppUserId);
                entity.HasOne(x => x.Room).WithMany(x => x.Users)
                    .HasForeignKey(x => x.RoomId);
                entity.ToTable("PlaningRoomUsers");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

        }
    }
}
