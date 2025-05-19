using BO.Models.CodeReviewApp.DAL.Domain;
using BO.Models.DAL.Domain;
using BO.Models.MenuApp.DAL.Domain;
using BO.Models.PlaningPoker.DAL;
using BO.Models.VaultApp.Dal;
using BO.Models.WordsCardsApp.DAL.Domain;
using DAL.Models.DAL.ContextSetup;
using DAL.Models.DAL.ContextSetup.CodeReview;
using DAL.Models.DAL.ContextSetup.MenuApp;
using DAL.Models.DAL.ContextSetup.PlaningPoker;
using DAL.Models.DAL.ContextSetup.VaultApp;
using DAL.Models.DAL.ContextSetup.WordsCards;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL
{
    public sealed class MenuDbContext : DbContext
    {
        #region main
        public DbSet<MainNLogEntity> MainLogTable { get; set; }

        public DbSet<Notification> Notifications { get; set; }

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
            modelBuilder.MainNLogEntityBuild();
            modelBuilder.UserBuild();
            modelBuilder.ArticleBuild();
            modelBuilder.CustomImageBuild();
            modelBuilder.NotificationBuild();


            #region wordscards
            modelBuilder.WordCardBuild();
            modelBuilder.WordsListBuild();
            #endregion wordscards
            //planingPoker

            #region planingPoker

            modelBuilder.PlaningRoomDalBuild();
            modelBuilder.PlaningRoomUserDalBuild();
            modelBuilder.PlaningStoryDalBuild();

            #endregion planingPoker


            #region codeReviewApp

            //modelBuilder.Entity<ProjectUser>().has
            modelBuilder.ProjectBuild();
            modelBuilder.ProjectUserBuild();
            modelBuilder.TaskReviewBuild();

            #endregion codeReviewApp


            #region VaultApp
            //vault

            modelBuilder.VaultBuild();
            modelBuilder.SecretBuild();
            modelBuilder.VaultUserDalBuild();



            #endregion VaultApp

            base.OnModelCreating(modelBuilder);
        }

    }
}
