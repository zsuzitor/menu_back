using BO.Models.FinancialAssistant.DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.FinancialAssistant
{
    public static class StockElementBuilder
    {
        public static ModelBuilder FinancialAssistantStockElementBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockElement>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Currency).WithMany()
                    .HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.Stock).WithMany()
                    .HasForeignKey(x => x.StockId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.Portfolio).WithMany(x=>x.Elements)
                    .HasForeignKey(x => x.PortfolioId).OnDelete(DeleteBehavior.NoAction);
                entity.ToTable("StockElement", schema: "FinancialAssistantApp");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

            return modelBuilder;

        }
    }
}
