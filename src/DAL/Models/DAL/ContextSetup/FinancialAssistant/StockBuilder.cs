using BO.Models.FinancialAssistant.DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.FinancialAssistant
{
    public static class StockBuilder
    {
        public static ModelBuilder FinancialAssistantStockBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Currency).WithMany()
                    .HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.Portfolio).WithMany()
                    .HasForeignKey(x => x.PortfolioId).OnDelete(DeleteBehavior.NoAction);
                entity.ToTable("Stock", schema: "FinancialAssistantApp");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты

            });

            return modelBuilder;

        }
    }
}
