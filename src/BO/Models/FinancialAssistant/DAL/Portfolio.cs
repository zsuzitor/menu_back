using BO.Models.DAL;
using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BO.Models.FinancialAssistant.DAL
{
    public class Portfolio : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long UserId { get; set; }
        public ProjectUser User { get; set; }


        public List<StockElement> Elements { get; set; }
        public List<StockEvent> Events { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
