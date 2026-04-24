using SignalRAuction.Data;
using SignalRAuction.Models;
using SignalRAuction.Services;
using System.Text.Json;

namespace SignalRAuctionTest
{
    [TestClass]
    public sealed class AuctionTest()
    {
        [TestMethod]
        public async Task ItemLifecycleTest()
        {
            ItemService itemService = new ItemService();

            // Add an item
            var newItem = new Item()
            {
                Name = "Test Item 1",
                Description = "Test Item Description",
                StartingBid = 1.00m
            };
            int newId = itemService.AddItem(newItem);
            newItem.Id = newId;

            // Get the list and check that the item exists in it
            var checkItemList = itemService.GetAllItems();
            Assert.IsTrue(checkItemList.Any(i => i.Id == newItem.Id), "Item was not found in the list after adding.");

            // Get the item
            var checkItem = itemService.GetItemById(newItem.Id);
            Assert.IsNotNull(checkItem, "Item was not found after adding.");
            Assert.AreEqual(newItem.Id, checkItem.Id, "Item has wrong data after adding.");
            Assert.AreEqual(newItem.Name, checkItem.Name, "Item has wrong data after adding.");
            Assert.AreEqual(newItem.Description, checkItem.Description, "Item has wrong data after adding.");
            Assert.AreEqual(newItem.StartingBid, checkItem.StartingBid, "Item has wrong data after adding.");
            Assert.AreEqual(newItem.StartingBid, checkItem.CurrentBid, "Item has wrong data after adding.");

            // Update the item
            checkItem.Name += " edited";
            checkItem.Description += "edited";
            checkItem.StartingBid += 1.00m;
            checkItem.ClosingTime = checkItem.ClosingTime.AddMinutes(1);
            checkItem.OriginalClosingTime = checkItem.OriginalClosingTime.AddMinutes(1); // Set closing time to 5 seconds in the future to prep for bid after time runs out
            itemService.UpdateItem(checkItem);
            var checkItem2 = itemService.GetItemById(newItem.Id);
            Assert.IsNotNull(checkItem2, "Item was not found after update.");
            Assert.AreEqual(checkItem.Id, checkItem2.Id, "Item has wrong data after update.");
            Assert.AreEqual(checkItem.Name, checkItem2.Name, "Item has wrong data after update.");
            Assert.AreEqual(checkItem.Description, checkItem2.Description, "Item has wrong data after update.");
            Assert.AreEqual(checkItem.StartingBid, checkItem2.StartingBid, "Item has wrong data after update.");
            Assert.AreEqual(checkItem.ClosingTime, checkItem2.ClosingTime, "Item has wrong data after update.");
            Assert.AreEqual(checkItem.OriginalClosingTime, checkItem2.OriginalClosingTime, "Item has wrong data after update.");

            // Place a bid on the item (while time remains)
            BidResponse response = itemService.PlaceBid(checkItem.Id, 0, checkItem.StartingBid + 1.00m);
            Assert.IsTrue(response.IsSuccessful, "Recieved unsuccessful response when bidding on an item before the closing time.");

            // Place a bid on the item (after time runs out)
            var checkItem3 = itemService.GetItemById(newItem.Id);
            Assert.IsNotNull(checkItem3, "Item no longer exists when preparing to update the closing time.");
            checkItem3.ClosingTime = DateTime.UtcNow;
            itemService.UpdateItem(checkItem3); // Update the item to close now
            await Task.Delay(2000); // Wait 2 seconds to be past the closing time
            response = itemService.PlaceBid(checkItem3.Id, 0, checkItem3.CurrentBid + 1.00m);
            Assert.IsFalse(response.IsSuccessful, "Received successful response when bidding on an item after the closing time.");

            // Delete the item
            var deleteResult = itemService.DeleteItem(checkItem3.Id);
            Assert.IsTrue(deleteResult, "Received 'false' when deleting the item.");
            checkItemList = itemService.GetAllItems();
            Assert.IsFalse(checkItemList.Any(i => i.Id == newItem.Id), "Item still exists in the list after successful delete.");
            var checkItem4 = itemService.GetItemById(newItem.Id);
            Assert.IsNull(checkItem4, "Item still exists after successful delete.");
        }

        [TestMethod]
        public async Task BiddingTest()
        {
            BidderService bidderService = new BidderService();
            ItemService itemService = new ItemService();

            // Prep the bidders and item
            Bidder bidder1 = new Bidder() { Name = "Bidder 1" };
            bidder1.Id = bidderService.AddBidder(bidder1.Name);
            Bidder bidder2 = new Bidder() { Name = "Bidder 2" };
            bidder2.Id = bidderService.AddBidder(bidder2.Name);
            var item = new Item()
            {
                Name = "Test Item 1",
                Description = "Test Item Description",
                StartingBid = 1.00m
            };
            item.Id = itemService.AddItem(item);

            // Place a bid on the item (while time remains)
            decimal bidAmount = item.StartingBid + 1.00m;
            var bidTime = DateTime.UtcNow;
            BidResponse response = itemService.PlaceBid(item.Id, bidder1.Id, bidAmount);
            Assert.IsTrue(response.IsSuccessful, "Recieved unsuccessful response when bidding on an item before the closing time.");
            item = itemService.GetItemById(item.Id);
            Assert.IsNotNull(item, "Item was not found after placing the first bid.");
            Assert.AreEqual(item.CurrentBid, bidAmount, "Bid amount is different after placing the first bid.");
            Assert.AreEqual(item.CurrentWinnerId, bidder1.Id, "Current winner ID is wrong after placing the first bid.");
            Assert.IsGreaterThanOrEqualTo(bidTime, item.ClosingTime, "Closing time is before bid time after placing the first bid.");

            // Place a second bid on the item (when time has almost run out)
            item = itemService.GetItemById(item.Id);
            Assert.IsNotNull(item, "Item no longer exists when preparing to update the closing time before the second bid.");
            item.ClosingTime = DateTime.UtcNow.AddSeconds(5);
            var prevClosingTime = item.ClosingTime;
            var updateResponse = itemService.UpdateItem(item); // Update the item to close now
            Assert.IsTrue(updateResponse, "Received 'false' when updating the item to set the closing time before the second bid.");
            bidAmount += 1.00m;
            bidTime = DateTime.UtcNow;
            response = itemService.PlaceBid(item.Id, bidder2.Id, bidAmount);
            Assert.IsTrue(response.IsSuccessful, "Recieved unsuccessful response when bidding on an item before the closing time.");
            item = itemService.GetItemById(item.Id);
            Assert.IsNotNull(item, "Item was not found after placing the second bid.");
            Assert.AreEqual(item.CurrentBid, bidAmount, "Bid amount is different after placing the second bid.");
            Assert.AreEqual(item.CurrentWinnerId, bidder2.Id, "Current winner ID is wrong after placing the second bid.");
            Assert.IsGreaterThan(prevClosingTime, item.ClosingTime, "Closing time was not extended after placing the second bid.");

            // Place a bid on the item (after time runs out)
            item = itemService.GetItemById(item.Id);
            Assert.IsNotNull(item, "Item no longer exists when preparing to update the closing time for the third bid attempt.");
            item.ClosingTime = DateTime.UtcNow;
            itemService.UpdateItem(item); // Update the item to close now
            await Task.Delay(2000); // Wait 2 seconds to be past the closing time
            response = itemService.PlaceBid(item.Id, bidder1.Id, item.CurrentBid + 1.00m);
            Assert.IsFalse(response.IsSuccessful, "Received successful response when bidding on an item after the closing time.");
            var checkItem = itemService.GetItemById(item.Id);
            Assert.IsTrue(updateResponse, "Item no longer exists after attempting to bid after the close time.");
            var itemJson = JsonSerializer.Serialize(item);
            var checkItemJson = JsonSerializer.Serialize(checkItem);
            Assert.AreEqual(itemJson, checkItemJson, "Item was changed while attempting to bid after the close time.");
        }
    }
}
