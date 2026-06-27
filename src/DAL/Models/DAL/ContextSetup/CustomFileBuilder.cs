using BO.Models.DAL.Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup
{
    internal static class CustomFileBuilder
    {
        public static void CustomFileBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomFile>(entity =>
            {
                entity.HasKey(x => x.Id);


                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
                entity.ToTable("Files");
            });

        }
    }
}
