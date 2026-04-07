namespace SignalRAuction.Data
{
    public class Item
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal StartingBid { get; set; } = 0;
        public decimal CurrentBid { get; set; } = 0;
        public int CurrentWinnerId { get; set; } = 0;
        public DateTime OriginalClosingTime { get; set; } = DateTime.Now.AddDays(1);
        public DateTime ClosingTime { get; set; } = DateTime.Now.AddDays(1);
    }
}
