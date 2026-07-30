using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAssistantApp.Models.DAL.Repositories.Interfaces
{
    public interface IStockEventRepository : IGeneralRepository<StockEvent, long>
    {
    }
}
