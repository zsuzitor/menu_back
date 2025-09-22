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
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.AdditionalImages)
                    .WithOne(x => x.Article)
                    .HasForeignKey(x => x.ArticleId).OnDelete(DeleteBehavior.Cascade);
                entity.ToTable("Articles");


                entity.Property(p => p.RowVersion)
                  .IsRowVersion() // Автоматически обновляется SQL Server
                  .IsConcurrencyToken(); // Включает проверку на конфликты
            });



        }
    }
}
