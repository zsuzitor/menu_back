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
    [Migration("20221121154049_room_img")]
    partial class room_img
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BO.Models.CodeReviewApp.DAL.Domain.CommentReview", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<long>("TaskId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("TaskId");

                    b.ToTable("ReviewComment");
                });

            modelBuilder.Entity("BO.Models.CodeReviewApp.DAL.Domain.Project", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ReviewProject");
                });

            modelBuilder.Entity("BO.Models.CodeReviewApp.DAL.Domain.ProjectUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deactivated")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<long?>("MainAppUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("NotifyEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ProjectId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MainAppUserId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ReviewProjectUsers");
                });

            modelBuilder.Entity("BO.Models.CodeReviewApp.DAL.Domain.TaskReview", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CreatorEntityId")
                        .HasColumnType("bigint");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ProjectId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ReviewerId")
                        .HasColumnType("bigint");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ReviewerId");

                    b.ToTable("ReviewTasks");
                });

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

            modelBuilder.Entity("BO.Models.DAL.Domain.MainNLogEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ActionDateEnd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ActionDateStart")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConnectionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EnteredDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogException")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogLevel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogLogger")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogStacktrace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("EnteredDate");

                    b.ToTable("MainLogTable");
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

            modelBuilder.Entity("BO.Models.PlaningPoker.DAL.PlaningRoomDal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cards")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("PlaningRooms");
                });

            modelBuilder.Entity("BO.Models.PlaningPoker.DAL.PlaningRoomUserDal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("MainAppUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Roles")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MainAppUserId");

                    b.HasIndex("RoomId");

                    b.ToTable("PlaningRoomUsers");
                });

            modelBuilder.Entity("BO.Models.PlaningPoker.DAL.PlaningStoryDal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Vote")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("PlaningStories");
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

            modelBuilder.Entity("BO.Models.CodeReviewApp.DAL.Domain.CommentReview", b =>
                {
                    b.HasOne("BO.Models.CodeReviewApp.DAL.Domain.ProjectUser", "Creator")
                        .WithMany("Comments")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BO.Models.CodeReviewApp.DAL.Domain.TaskReview", "Task")
                        .WithMany("Comments")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BO.Models.CodeReviewApp.DAL.Domain.ProjectUser", b =>
                {
                    b.HasOne("BO.Models.DAL.Domain.User", "MainAppUser")
                        .WithMany("CodeReviewProjects")
                        .HasForeignKey("MainAppUserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("BO.Models.CodeReviewApp.DAL.Domain.Project", "Project")
                        .WithMany("Users")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BO.Models.CodeReviewApp.DAL.Domain.TaskReview", b =>
                {
                    b.HasOne("BO.Models.CodeReviewApp.DAL.Domain.ProjectUser", "Creator")
                        .WithMany("CreateByUser")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BO.Models.CodeReviewApp.DAL.Domain.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BO.Models.CodeReviewApp.DAL.Domain.ProjectUser", "Reviewer")
                        .WithMany("ReviewByUser")
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.NoAction);
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

            modelBuilder.Entity("BO.Models.PlaningPoker.DAL.PlaningRoomUserDal", b =>
                {
                    b.HasOne("BO.Models.DAL.Domain.User", "MainAppUser")
                        .WithMany()
                        .HasForeignKey("MainAppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BO.Models.PlaningPoker.DAL.PlaningRoomDal", "Room")
                        .WithMany("Users")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BO.Models.PlaningPoker.DAL.PlaningStoryDal", b =>
                {
                    b.HasOne("BO.Models.PlaningPoker.DAL.PlaningRoomDal", "Room")
                        .WithMany("Stories")
                        .HasForeignKey("RoomId")
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
