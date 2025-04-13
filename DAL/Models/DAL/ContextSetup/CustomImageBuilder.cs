using BO.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup
{
    internal static class CustomImageBuilder
    {
        public static void CustomImageBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomImage>().HasKey(x => x.Id);
            //modelBuilder.Entity<CustomImage>().HasOne(x => x.Article);

            modelBuilder.Entity<CustomImage>().ToTable("Images");
        }
    }
}
