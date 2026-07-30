using BO.Models.FinancialAssistant.DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.DAL.ContextSetup.FinancialAssistant
{
    public static class PortfolioBuilder
    {
        public static ModelBuilder FinancialAssistantPortfolioBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Portfolio>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.User).WithMany()
                    .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.Currency).WithMany()
                    .HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.NoAction);
                entity.ToTable("Portfolio", schema: "FinancialAssistantApp");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты

            });

            return modelBuilder;

        }
    }
}

