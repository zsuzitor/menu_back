using BO.Models.FinancialAssistant.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DAL.ContextSetup.FinancialAssistant
{
    public static class StockHistoryBuilder
    {
        public static ModelBuilder FinancialAssistantStockHistoryBuild(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockHistory>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Currency).WithMany()
                    .HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.Stock).WithMany(x=>x.StockHistory)
                    .HasForeignKey(x => x.StockId).OnDelete(DeleteBehavior.NoAction);

                entity.ToTable("StockHistory", schema: "FinancialAssistantApp");

                entity.Property(p => p.RowVersion)
                    .IsRowVersion() // Автоматически обновляется SQL Server
                    .IsConcurrencyToken(); // Включает проверку на конфликты
            });

            return modelBuilder;

        }
    }
}
