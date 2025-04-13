using BO.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup
{
    internal static class UserBuilder
    {
        public static void UserBuild(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.Email).IsRequired();
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.Login).IsRequired();
            //TODO раскомментить если будет изменение логики заполнения логинов, тк сейчас полностью копирует почту, смысла ноый индекс делать нет
            //modelBuilder.Entity<User>().HasIndex(x => x.Login).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.PasswordHash).IsRequired();


            modelBuilder.Entity<User>().HasMany(x => x.Articles).WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>().HasMany(x => x.WordsCards).WithOne(x => x.User).
                HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>().HasMany(x => x.WordsLists).WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);


            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
