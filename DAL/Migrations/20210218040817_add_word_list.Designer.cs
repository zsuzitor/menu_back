﻿// <auto-generated />
using System;
using DAL.Models.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(MenuDbContext))]
    [Migration("20210218040817_add_word_list")]
    partial class add_word_list
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BO.Models.DAL.Domain.CustomImage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ArticleId")
                        .HasColumnType("bigint");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("BO.Models.DAL.Domain.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshTokenHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BO.Models.MenuApp.DAL.Domain.Article", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Followed")
                        .HasColumnType("bit");

                    b.Property<string>("MainImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("BO.Models.WordsCardsApp.DAL.Domain.WordCard", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Hided")
                        .HasColumnType("bit");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Word")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WordAnswer")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("WordsCards");
                });

            modelBuilder.Entity("BO.Models.WordsCardsApp.DAL.Domain.WordCardWordList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("WordCardId")
                        .HasColumnType("bigint");

                    b.Property<long>("WordsListId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("WordCardId");

                    b.HasIndex("WordsListId");

                    b.ToTable("WordCardWordList");
                });

            modelBuilder.Entity("BO.Models.WordsCardsApp.DAL.Domain.WordsList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("WordsLists");
                });

            modelBuilder.Entity("BO.Models.DAL.Domain.CustomImage", b =>
                {
                    b.HasOne("BO.Models.MenuApp.DAL.Domain.Article", "Article")
                        .WithMany("AdditionalImages")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BO.Models.MenuApp.DAL.Domain.Article", b =>
                {
                    b.HasOne("BO.Models.DAL.Domain.User", "User")
                        .WithMany("Articles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BO.Models.WordsCardsApp.DAL.Domain.WordCard", b =>
                {
                    b.HasOne("BO.Models.DAL.Domain.User", "User")
                        .WithMany("WordsCards")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("BO.Models.WordsCardsApp.DAL.Domain.WordCardWordList", b =>
                {
                    b.HasOne("BO.Models.WordsCardsApp.DAL.Domain.WordCard", "WordCard")
                        .WithMany("WordCardWordList")
                        .HasForeignKey("WordCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BO.Models.WordsCardsApp.DAL.Domain.WordsList", "WordsList")
                        .WithMany("WordCardWordList")
                        .HasForeignKey("WordsListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BO.Models.WordsCardsApp.DAL.Domain.WordsList", b =>
                {
                    b.HasOne("BO.Models.DAL.Domain.User", "User")
                        .WithMany("WordsLists")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
