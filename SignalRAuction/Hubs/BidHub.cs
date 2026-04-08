using Microsoft.AspNetCore.SignalR;
using SignalRAuction.Data;
using SignalRAuction.Models;
using SignalRAuction.Services;

namespace SignalRAuction.Hubs
{
    public class BidHub(IItemService itemService, IBidderService bidderService): Hub
    {
        public async Task ConnectBidder(string name)
        {
            // Create a new bidder and return the ID
            int id = bidderService.AddBidder(name);

            // Initialize the auction if there are no items with closing times in the future
            var items = itemService.GetAllItems();
            if (!items.Any(i => i.ClosingTime > DateTime.UtcNow.AddMinutes(1)))
            {
                itemService.Reset();
            }

            // Send a full sync of items to the new bidder
            var syncMessage = GenerateSync(id);
            await Clients.Caller.SendAsync("ReceiveSync", syncMessage);
        }

        public async Task ReconnectBidder(int bidderId)
        {
            // Send a full sync of items to the re-connected bidder
            var syncMessage = GenerateSync(bidderId);
            await Clients.Caller.SendAsync("ReceiveSync", syncMessage);
        }

        public async Task SubmitBid(SubmitBidRequest request)
        {
            var bidResponse = itemService.PlaceBid(request.ItemId, request.BidderId, request.BidAmount);
            if (bidResponse.IsSuccessful)
            {
                var item = itemService.GetItemById(request.ItemId);
                if (item == null)
                {
                    return;
                }
                var bidder = bidderService.GetBidderById(request.BidderId);
                SyncItem syncItem = new SyncItem()
                {
                    itemId = item.Id,
                    name = item.Name,
                    description = item.Description,
                    currentBid = item.CurrentBid,
                    currentWinnerId = item.CurrentWinnerId,
                    closingTime = item.ClosingTime,
                };
                await BroadcastBid(syncItem);
            }
        }

        public async Task BroadcastBid(SyncItem syncItem)
        {
            await Clients.All.SendAsync("ReceiveBid", syncItem);
        }

        public async Task SyncAuction(int bidderId)
        {
            await Clients.Caller.SendAsync("ReceiveSync", GenerateSync(bidderId));
        }

        private SyncMessage GenerateSync(int bidderId)
        {
            List<SyncItem> syncItems = new List<SyncItem>();
            var items = itemService.GetAllItems();
            foreach (var item in items ?? [])
            {
                syncItems.Add(new SyncItem()
                {
                    itemId = item.Id,
                    name = item.Name,
                    description = item.Description,
                    currentBid = item.CurrentBid,
                    currentWinnerId = item.CurrentWinnerId,
                    closingTime = item.ClosingTime,
                });
            }
            var syncMessage = new SyncMessage()
            {
                bidderId = bidderId,
                bidderName = bidderService.GetBidderById(bidderId)?.Name ?? "",
                items = syncItems.ToArray()
            };
            return syncMessage;
        }
    }
}
