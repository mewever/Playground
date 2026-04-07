using SignalRAuction.Data;
using SignalRAuction.Models;

namespace SignalRAuction.Services
{
    public class ItemService() : IItemService
    {
        private List<Item> _items = [
            new Item() { Id = 1, Name = "Item 1", Description = "Item 1", StartingBid = 1, CurrentBid = 1, ClosingTime = DateTime.UtcNow.AddMinutes(1), OriginalClosingTime = DateTime.UtcNow },
            new Item() { Id = 2, Name = "Item 2", Description = "Item 2", StartingBid = 1, CurrentBid = 1, ClosingTime = DateTime.UtcNow.AddMinutes(2), OriginalClosingTime = DateTime.UtcNow.AddMinutes(1) },
            new Item() { Id = 3, Name = "Item 3", Description = "Item 3", StartingBid = 1, CurrentBid = 1, ClosingTime = DateTime.UtcNow.AddMinutes(4), OriginalClosingTime = DateTime.UtcNow.AddMinutes(3) },
            new Item() { Id = 4, Name = "Item 4", Description = "Item 4", StartingBid = 1, CurrentBid = 1, ClosingTime = DateTime.UtcNow.AddMinutes(3), OriginalClosingTime = DateTime.UtcNow.AddMinutes(2) },
            ];
        private List<Bid> _bidHistory = [];
        private int _nextItemId = 1;
        private int _nextBidId = 1;

        public List<Item> GetAllItems()
        {
            return _items;
        }

        public Item? GetItemById(int itemId)
        {
            return _items.FirstOrDefault(i => i.Id == itemId);
        }

        public int AddItem(Item item)
        {
            int newId = _nextItemId++;
            var newItem = new Item
            {
                Id = newId,
                Name = item.Name,
                Description = item.Description,
                StartingBid = item.StartingBid,
                CurrentBid = item.StartingBid,
                CurrentWinnerId = 0,
                ClosingTime = item.ClosingTime,
                OriginalClosingTime = item.ClosingTime,
            };
            _items.Add(newItem);
            return newId;
        }

        public bool UpdateItem(Item item)
        {
            var existingItem = _items.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem == null)
            {
                return false;
            }
            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.StartingBid = item.StartingBid;
            existingItem.ClosingTime = item.ClosingTime;
            existingItem.OriginalClosingTime = item.ClosingTime;
            return true;
        }

        public bool DeleteItem(int itemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return false;
            }
            _items.Remove(item);
            return true;
        }

        public BidResponse PlaceBid(int itemId, int bidderId, decimal bidAmount)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return new BidResponse { IsSuccessful = false, Message = "Item not found." };
            }
            if (DateTime.UtcNow > item.ClosingTime)
            {
                return new BidResponse { IsSuccessful = false, Message = "Bidding has closed for this item." };
            }
            if (bidAmount <= item.CurrentBid)
            {
                return new BidResponse { IsSuccessful = false, Message = $"Bid must be higher than current bid of {item.CurrentBid}."};
            }
            item.CurrentBid = bidAmount;
            item.CurrentWinnerId = bidderId;
            Bid newBid = new Bid
            {
                Id = _nextBidId++,
                ItemId = itemId,
                BidderId = bidderId,
                Amount = bidAmount,
                Timestamp = DateTime.UtcNow
            };
            _bidHistory.Add(newBid);

            // Extend bidding time by 1 minute from now if bid placed when less than a minute left
            if (item.ClosingTime - DateTime.UtcNow <= TimeSpan.FromMinutes(1))
            {
                item.ClosingTime = DateTime.UtcNow.AddMinutes(1);
            }

            return new BidResponse { IsSuccessful = true, Message = "Bid placed successfully." };
        }
    }
}
