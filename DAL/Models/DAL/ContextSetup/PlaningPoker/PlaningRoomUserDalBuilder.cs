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


            modelBuilder.Entity<PlaningRoomUserDal>().HasKey(x => x.Id);
            modelBuilder.Entity<PlaningRoomUserDal>().Property(x => x.MainAppUserId).IsRequired();
            modelBuilder.Entity<PlaningRoomUserDal>().HasOne(x => x.MainAppUser).WithMany()
                .HasForeignKey(x => x.MainAppUserId);
            modelBuilder.Entity<PlaningRoomUserDal>().HasOne(x => x.Room).WithMany(x => x.Users)
                .HasForeignKey(x => x.RoomId);

            modelBuilder.Entity<PlaningRoomUserDal>().ToTable("PlaningRoomUsers");
        }
    }
}
