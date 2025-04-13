using BO.Models.DAL.Domain;
using BO.Models.MenuApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.MenuApp
{
    internal static class ArticleBuilder
    {
        public static void ArticleBuild(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Article>().HasKey(x => x.Id);
            modelBuilder.Entity<Article>().HasMany(x => x.AdditionalImages)
                .WithOne(x => x.Article)
                .HasForeignKey(x => x.ArticleId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Article>().ToTable("Articles");

        }
    }
}
