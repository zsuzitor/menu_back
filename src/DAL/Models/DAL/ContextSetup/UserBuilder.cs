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
            modelBuilder.Entity<User>(entity => {
            entity.HasKey(x => x.Id);
                entity.Property(x => x.Email).IsRequired();
                entity.HasIndex(x => x.Email).IsUnique();
                entity.Property(x => x.Login).IsRequired();
                //раскомментить если будет изменение логики заполнения логинов, тк сейчас полностью копирует почту, смысла ноый индекс делать нет
                //modelBuilder.Entity<User>().HasIndex(x => x.Login).IsUnique();
                entity.Property(x => x.PasswordHash).IsRequired();
                entity.HasMany(x => x.Articles).WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
                entity.HasMany(x => x.WordsCards).WithOne(x => x.User).
                HasForeignKey(x => x.UserId);
                entity.HasMany(x => x.WordsLists).WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
                entity.ToTable("Users");

            });
        }
    }
}
