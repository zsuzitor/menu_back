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

            modelBuilder.Entity<PlaningRoomDal>().HasKey(x => x.Id);
            modelBuilder.Entity<PlaningRoomDal>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<PlaningRoomDal>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<PlaningRoomDal>().HasMany(x => x.Stories).WithOne(x => x.Room)
                .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PlaningRoomDal>().HasMany(x => x.Users).WithOne(x => x.Room)
                .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaningRoomDal>().ToTable("PlaningRooms");
        }
    }
}
