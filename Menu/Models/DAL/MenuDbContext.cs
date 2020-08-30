using Menu.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace Menu.Models.DAL
{
    public class MenuDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CustomImage> Images { get; set; }


        public MenuDbContext(DbContextOptions<MenuDbContext> options)
               : base(options)
        {
            //Database.EnsureCreated();//создаст БД если ее нет, это вроде лучше не юзать
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasMany(x => x.Articles).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>().HasMany(x => x.Articles).WithOne(x => x.User).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Article>().HasKey(x => x.Id);
            modelBuilder.Entity<Article>().HasMany(x => x.AdditionalImages).WithOne(x => x.Article).HasForeignKey(x => x.ArticleId);

            //modelBuilder.Entity<CustomImage>().HasOne(x => x.Article);



            base.OnModelCreating(modelBuilder);
        }

    }
}
