using SignalRAuction.Data;

namespace SignalRAuction.Models
{
    public class SyncItem
    {
        public int itemId { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public decimal currentBid { get; set; } = 0;
        public int currentWinnerId { get; set; } = 0;
        public DateTime closingTime { get; set; } = DateTime.Now.AddDays(1);
    }
}
