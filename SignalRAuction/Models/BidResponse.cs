namespace SignalRAuction.Models
{
    public class BidResponse
    {
        public bool IsSuccessful { get; set; } = false;
        public string? Message { get; set; }
    }
}
