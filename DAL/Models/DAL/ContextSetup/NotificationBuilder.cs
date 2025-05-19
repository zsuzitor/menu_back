using BO.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup
{
    internal static class NotificationBuilder
    {
        public static void NotificationBuild(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Notification>().HasKey(x => x.Id);
            //modelBuilder.Entity<CustomImage>().HasOne(x => x.Article);

            modelBuilder.Entity<Notification>().ToTable("Notifications");
        }
    }
}
