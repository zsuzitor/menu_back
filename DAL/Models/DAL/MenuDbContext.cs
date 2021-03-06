﻿using BO.Models.DAL.Domain;
using BO.Models.MenuApp.DAL.Domain;
using BO.Models.PlaningPoker.DAL;
using BO.Models.WordsCardsApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL
{
    public class MenuDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CustomImage> Images { get; set; }
        public DbSet<WordCard> WordsCards { get; set; }
        public DbSet<WordsList> WordsLists { get; set; }


        public DbSet<WordCardWordList> WordCardWordList { get; set; }


        public DbSet<PlaningRoomDal> PlaningRooms { get; set; }
        public DbSet<PlaningRoomUserDal> PlaningRoomUsers { get; set; }
        public DbSet<PlaningStoryDal> PlaningStories { get; set; }




        public MenuDbContext(DbContextOptions<MenuDbContext> options)
               : base(options)
        {
            //Database.EnsureCreated();//создаст БД если ее нет, это вроде лучше не юзать
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.Email).IsRequired();
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.Login).IsRequired();
            //TODO раскомментить если будет изменение логики заполнения логинов, тк сейчас полностью копирует почту, смысла ноый индекс делать нет
            //modelBuilder.Entity<User>().HasIndex(x => x.Login).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.PasswordHash).IsRequired();


            modelBuilder.Entity<User>().HasMany(x => x.Articles).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>().HasMany(x => x.WordsCards).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>().HasMany(x => x.WordsLists).WithOne(x => x.User).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Article>().HasKey(x => x.Id);
            modelBuilder.Entity<Article>().HasMany(x => x.AdditionalImages).WithOne(x => x.Article).HasForeignKey(x => x.ArticleId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomImage>().HasKey(x => x.Id);
            //modelBuilder.Entity<CustomImage>().HasOne(x => x.Article);

            modelBuilder.Entity<WordCard>().HasKey(x => x.Id);
            modelBuilder.Entity<WordCard>().HasMany(x => x.WordCardWordList).WithOne(x => x.WordCard).HasForeignKey(x => x.WordCardId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WordsList>().HasKey(x => x.Id);
            modelBuilder.Entity<WordsList>().HasMany(x => x.WordCardWordList).WithOne(x => x.WordsList).HasForeignKey(x => x.WordsListId).OnDelete(DeleteBehavior.Cascade);

            //many to many
            modelBuilder.Entity<WordCardWordList>().HasKey(x => x.Id);


            //planingPoker
            #region planingPoker
            modelBuilder.Entity<PlaningRoomDal>().HasKey(x => x.Id);
            modelBuilder.Entity<PlaningRoomDal>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<PlaningRoomDal>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<PlaningRoomDal>().HasMany(x => x.Stories).WithOne(x => x.Room).HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PlaningRoomDal>().HasMany(x => x.Users).WithOne(x => x.Room).HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<PlaningRoomUserDal>().HasKey(x => x.Id);
            modelBuilder.Entity<PlaningRoomUserDal>().Property(x => x.MainAppUserId).IsRequired();
            modelBuilder.Entity<PlaningRoomUserDal>().HasOne(x => x.MainAppUser).WithMany().HasForeignKey(x => x.MainAppUserId);
            modelBuilder.Entity<PlaningRoomUserDal>().HasOne(x => x.Room).WithMany(x => x.Users).HasForeignKey(x => x.RoomId);

            modelBuilder.Entity<PlaningStoryDal>().HasKey(x => x.Id);
            modelBuilder.Entity<PlaningStoryDal>().HasOne(x => x.Room).WithMany(x => x.Stories).HasForeignKey(x => x.RoomId);

            #endregion planingPoker


            base.OnModelCreating(modelBuilder);
        }

    }
}
