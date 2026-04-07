using SignalRAuction.Data;

namespace SignalRAuction.Services
{
    public interface IBidderService
    {
        public List<Bidder> GetAllBidders();
        public Bidder? GetBidderById(int itemId);
        public int AddBidder(string name);
        public bool UpdateBidder(Bidder bidder);
        public bool DeleteBidder(int bidderId);
    }
}
