namespace Menu.Host.Models.FinancialAssistantApp.Requests
{
    public class DeleteStockEventRequest
    {
        public long Id { get; set; }
        public bool Force { get; set; }
    }
}
