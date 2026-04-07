namespace SignalRAuction.Models
{
    public class SubmitBidRequest
    {
        public int BidderId { get; set; }
        public int ItemId { get; set; }
        public decimal BidAmount { get; set; }
    }
}
