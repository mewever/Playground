namespace SignalRAuction.Models
{
    public class SyncMessage
    {
        public int bidderId { get; set; }
        public string bidderName { get; set; } = string.Empty;
        public SyncItem[] items { get; set; } = [];
    }
}
