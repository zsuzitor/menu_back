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
            modelBuilder.Entity<PlaningStoryDal>().HasKey(x => x.Id);
            modelBuilder.Entity<PlaningStoryDal>().HasOne(x => x.Room).WithMany(x => x.Stories)
                .HasForeignKey(x => x.RoomId);

            modelBuilder.Entity<PlaningStoryDal>().Property(e => e.Vote)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<PlaningStoryDal>().ToTable("PlaningStories");

        }
    }
}
