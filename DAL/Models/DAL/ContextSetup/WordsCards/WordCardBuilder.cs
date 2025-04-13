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


            modelBuilder.Entity<WordCard>().HasKey(x => x.Id);
            modelBuilder.Entity<WordCard>().HasMany(x => x.WordCardWordList)
                .WithOne(x => x.WordCard)
                .HasForeignKey(x => x.WordCardId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WordCard>().ToTable("WordsCards");
        }
    }
}
