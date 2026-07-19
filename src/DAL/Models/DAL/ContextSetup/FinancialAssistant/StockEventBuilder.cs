using BO.Models.FinancialAssistant.DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.FinancialAssistant
{
    public static class StockEventBuilder
    {
        public static ModelBuilder FinancialAssistantStockEventBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockEvent>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Currency).WithMany()
                    .HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.StockElement).WithMany(x=>x.Events)
                    .HasForeignKey(x => x.StockElementId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.Portfolio).WithMany(x => x.Events)
                    .HasForeignKey(x => x.PortfolioId).OnDelete(DeleteBehavior.NoAction);
                entity.ToTable("StockEvent", schema: "FinancialAssistantApp");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

            return modelBuilder;

        }
    }
}
