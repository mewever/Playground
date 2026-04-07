namespace SignalRAuction.Data
{
    public class Bid
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int BidderId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Amount { get; set; }
    }
}
