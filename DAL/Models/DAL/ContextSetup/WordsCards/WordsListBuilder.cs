using BO.Models.DAL.Domain;
using BO.Models.WordsCardsApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.WordsCards
{
    internal static class WordsListBuilder
    {
        public static void WordsListBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordsList>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.WordCardWordList)
                    .WithOne(x => x.WordsList)
                    .HasForeignKey(x => x.WordsListId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
                entity.ToTable("WordsLists");
            });


            //many to many
            modelBuilder.Entity<WordCardWordList>().HasKey(x => x.Id);



        }
    }
}
