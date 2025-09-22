using BO.Models.PlaningPoker.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.PlaningPoker
{
    internal static class PlaningRoomDalBuilder
    {
        public static void PlaningRoomDalBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaningRoomDal>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired();
                entity.HasIndex(x => x.Name).IsUnique();
                entity.HasMany(x => x.Stories).WithOne(x => x.Room)
                    .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.Users).WithOne(x => x.Room)
                    .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);
                entity.ToTable("PlaningRooms");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });


        }
    }
}
