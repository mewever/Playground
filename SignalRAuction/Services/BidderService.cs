using SignalRAuction.Data;

namespace SignalRAuction.Services
{
    public class BidderService: IBidderService
    {
        private List<Bidder> _bidders = [];
        private int _nextId = 1;

        public List<Bidder> GetAllBidders()
        {
            return _bidders;
        }

        public Bidder? GetBidderById(int itemId)
        {
            return _bidders.FirstOrDefault(b => b.Id == itemId);
        }

        public int AddBidder(string name)
        {
            Bidder bidder = new Bidder()
            {
                Id = _nextId++,
                Name = name
            };
            _bidders.Add(bidder);
            return bidder.Id;
        }

        public bool UpdateBidder(Bidder bidder)
        {
            var existingBidder = _bidders.FirstOrDefault(b => b.Id == bidder.Id);
            if (existingBidder == null)
            {
                return false;
            }
            existingBidder.Name = bidder.Name;
            return true;
        }

        public bool DeleteBidder(int bidderId)
        {
            var bidder = _bidders.FirstOrDefault(b => b.Id == bidderId);
            if (bidder == null)
            {
                return false;
            }
            _bidders.Remove(bidder);
            return true;
        }
    }
}
