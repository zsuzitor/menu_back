using BO.Models.CodeReviewApp.DAL.Domain;
using BO.Models.DAL.Domain;
using BO.Models.MenuApp.DAL.Domain;
using BO.Models.PlaningPoker.DAL;
using BO.Models.VaultApp.Dal;
using BO.Models.WordsCardsApp.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL
{
    public sealed class MenuDbContext : DbContext
    {
        #region main
        public DbSet<MainNLogEntity> MainLogTable { get; set; }
        #endregion main



        #region menu
        public DbSet<User> Users { get; set; }
        public DbSet<CustomImage> Images { get; set; }
        #endregion menu

        #region menuApp
        public DbSet<Article> Articles { get; set; }

        #endregion menuApp


        #region WordsCardsApp
        public DbSet<WordCard> WordsCards { get; set; }
        public DbSet<WordsList> WordsLists { get; set; }

        public DbSet<WordCardWordList> WordCardWordList { get; set; }
        #endregion WordsCardsApp



        #region PlaningPoker
        public DbSet<PlaningRoomDal> PlaningRooms { get; set; }
        public DbSet<PlaningRoomUserDal> PlaningRoomUsers { get; set; }
        public DbSet<PlaningStoryDal> PlaningStories { get; set; }
        #endregion PlaningPoker


        #region codeReviewApp
        public DbSet<Project> ReviewProject { get; set; }
        public DbSet<ProjectUser> ReviewProjectUsers { get; set; }
        public DbSet<TaskReview> ReviewTasks { get; set; }
        public DbSet<CommentReview> ReviewComment { get; set; }


        #endregion codeReviewApp

        #region VaultApp
        public DbSet<Vault> Vaults { get; set; }
        public DbSet<Secret> Secrets { get; set; }
        public DbSet<VaultUserDal> VaultUsers { get; set; }

        #endregion VaultApp    



        public MenuDbContext(DbContextOptions<MenuDbContext> options)
               : base(options)
        {
            //Database.EnsureCreated();//создаст БД если ее нет, это вроде лучше не юзать
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MainNLogEntity>().HasKey(x => x.Id).IsClustered(false);
            modelBuilder.Entity<MainNLogEntity>().Property(x => x.Id).HasDefaultValueSql("NEWID()");


            modelBuilder.Entity<MainNLogEntity>().HasIndex(x => x.EnteredDate);
            //https://docs.microsoft.com/ru-ru/ef/core/modeling/generated-properties?tabs=fluent-api
            modelBuilder.Entity<MainNLogEntity>().Property(x => x.EnteredDate).HasDefaultValueSql("GETDATE()");
            //modelBuilder.Entity<MainNLogEntity>().Property(x => x.EnteredDate).ValueGeneratedOnAdd();
            //    //.HasDefaultValue(3);
            //    .HasDefaultValueSql("getdate()");
            //    //.ValueGeneratedOnAdd
            modelBuilder.Entity<MainNLogEntity>().ToTable("MainLogTable");



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

            modelBuilder.Entity<Article>().HasKey(x => x.Id);
            modelBuilder.Entity<Article>().HasMany(x => x.AdditionalImages)
                .WithOne(x => x.Article)
                .HasForeignKey(x => x.ArticleId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomImage>().HasKey(x => x.Id);
            //modelBuilder.Entity<CustomImage>().HasOne(x => x.Article);

            modelBuilder.Entity<WordCard>().HasKey(x => x.Id);
            modelBuilder.Entity<WordCard>().HasMany(x => x.WordCardWordList)
                .WithOne(x => x.WordCard)
                .HasForeignKey(x => x.WordCardId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WordsList>().HasKey(x => x.Id);
            modelBuilder.Entity<WordsList>().HasMany(x => x.WordCardWordList)
                .WithOne(x => x.WordsList)
                .HasForeignKey(x => x.WordsListId).OnDelete(DeleteBehavior.Cascade);

            //many to many
            modelBuilder.Entity<WordCardWordList>().HasKey(x => x.Id);


            //planingPoker

            #region planingPoker

            modelBuilder.Entity<PlaningRoomDal>().HasKey(x => x.Id);
            modelBuilder.Entity<PlaningRoomDal>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<PlaningRoomDal>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<PlaningRoomDal>().HasMany(x => x.Stories).WithOne(x => x.Room)
                .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PlaningRoomDal>().HasMany(x => x.Users).WithOne(x => x.Room)
                .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<PlaningRoomUserDal>().HasKey(x => x.Id);
            modelBuilder.Entity<PlaningRoomUserDal>().Property(x => x.MainAppUserId).IsRequired();
            modelBuilder.Entity<PlaningRoomUserDal>().HasOne(x => x.MainAppUser).WithMany()
                .HasForeignKey(x => x.MainAppUserId);
            modelBuilder.Entity<PlaningRoomUserDal>().HasOne(x => x.Room).WithMany(x => x.Users)
                .HasForeignKey(x => x.RoomId);

            modelBuilder.Entity<PlaningStoryDal>().HasKey(x => x.Id);
            modelBuilder.Entity<PlaningStoryDal>().HasOne(x => x.Room).WithMany(x => x.Stories)
                .HasForeignKey(x => x.RoomId);

            #endregion planingPoker


            #region coreReviewApp
            modelBuilder.Entity<Project>().HasKey(x => x.Id);
            modelBuilder.Entity<Project>().HasMany(x => x.Tasks).WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Project>().HasMany(x => x.Users).WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ProjectUser>().has
            modelBuilder.Entity<User>().HasMany(x => x.CodeReviewProjects).WithOne(x => x.MainAppUser)
                .HasForeignKey(x => x.MainAppUserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProjectUser>().HasMany(x => x.Comments).WithOne(x => x.Creator)
                .HasForeignKey(x => x.CreatorId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProjectUser>().HasMany(x => x.CreateByUser).WithOne(x => x.Creator)
                .HasForeignKey(x => x.CreatorId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProjectUser>().HasMany(x => x.ReviewByUser).WithOne(x => x.Reviewer)
                .HasForeignKey(x => x.ReviewerId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TaskReview>().HasMany(x => x.Comments).WithOne(x => x.Task)
                .HasForeignKey(x => x.TaskId).OnDelete(DeleteBehavior.Cascade);

            #endregion coreReviewApp


            #region VaultApp
            //vault
            modelBuilder.Entity<Vault>().HasKey(x => x.Id);
            modelBuilder.Entity<Vault>().HasMany(x => x.Secrets).WithOne(x => x.Vault)
                .HasForeignKey(x => x.VaultId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Vault>().HasMany(x => x.Users).WithOne(x => x.Vault)
                .HasForeignKey(x => x.VaultId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(x => x.Vaults).WithOne(x => x.User)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<VaultUserDal>().HasKey(x => x.Id);
            modelBuilder.Entity<VaultUserDal>().HasIndex(x => x.UserId);
            modelBuilder.Entity<VaultUserDal>().HasIndex(x => x.VaultId);
            modelBuilder.Entity<VaultUserDal>().HasOne(x => x.Vault).WithMany(x => x.Users)
                .HasForeignKey(x => x.VaultId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Secret>().HasKey(x => x.Id);
            modelBuilder.Entity<Secret>().HasIndex(x => x.VaultId);
            modelBuilder.Entity<Secret>().Property(x => x.Key).IsRequired();
            modelBuilder.Entity<Secret>().HasOne(x => x.Vault).WithMany(x => x.Secrets);

            #endregion VaultApp

            base.OnModelCreating(modelBuilder);
        }

    }
}
