using BO.Models.WordsCardsApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.WordsCards
{
    internal static class WordCardBuilder
    {
        public static void WordCardBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordCard>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.WordCardWordList)
                    .WithOne(x => x.WordCard)
                    .HasForeignKey(x => x.WordCardId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
                entity.ToTable("WordsCards");
            });

        }
    }
}
