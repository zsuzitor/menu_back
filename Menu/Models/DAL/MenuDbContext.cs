using Menu.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace Menu.Models.DAL
{
    public class MenuDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }


        public MenuDbContext(DbContextOptions<MenuDbContext> options)
               : base(options)
        {
            //Database.EnsureCreated();//создаст БД если ее нет, это вроде лучше не юзать
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x=>x.Id);
            modelBuilder.Entity<User>().HasMany(x=>x.Articles);


            modelBuilder.Entity<Article>().HasKey(x => x.Id);
            modelBuilder.Entity<Article>().HasOne(x => x.User);


            
            base.OnModelCreating(modelBuilder);
        }

    }
}
