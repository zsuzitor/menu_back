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

            modelBuilder.Entity<WordsList>().HasKey(x => x.Id);
            modelBuilder.Entity<WordsList>().HasMany(x => x.WordCardWordList)
                .WithOne(x => x.WordsList)
                .HasForeignKey(x => x.WordsListId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<WordsList>().ToTable("WordsLists");

            //many to many
            modelBuilder.Entity<WordCardWordList>().HasKey(x => x.Id);



        }
    }
}
