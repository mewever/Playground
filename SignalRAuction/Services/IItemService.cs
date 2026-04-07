using SignalRAuction.Data;
using SignalRAuction.Models;

namespace SignalRAuction.Services
{
    public interface IItemService
    {
        public List<Data.Item> GetAllItems();
        public Data.Item? GetItemById(int itemId);
        public int AddItem(Item item);
        public bool UpdateItem(Item item);
        public bool DeleteItem(int itemId);
        public BidResponse PlaceBid(int itemId, int userId, decimal bidAmount);
    }
}
