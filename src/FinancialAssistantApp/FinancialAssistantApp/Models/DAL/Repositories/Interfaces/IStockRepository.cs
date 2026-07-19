using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAssistantApp.Models.DAL.Repositories.Interfaces
{
    public interface IStockRepository : IGeneralRepository<Stock, long>
    {

        Task<List<Stock>> FindAsync(string text);
        Task<List<Stock>> GetGlobalAsync();
        Task<List<Stock>> GetGlobalForActualiztionAsync(DateTime date);
    }
}
