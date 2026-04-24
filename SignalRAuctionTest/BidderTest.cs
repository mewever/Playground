using SignalRAuction.Data;
using SignalRAuction.Services;

namespace SignalRAuctionTest;

[TestClass]
public class BidderTest
{
    [TestMethod]
    public void BidderLifecycleTest()
    {
        BidderService bidderService = new BidderService();

        // Add a bidder
        Bidder newBidder = new Bidder()
        {
            Name = "Test Bidder"
        };
        int newId = bidderService.AddBidder(newBidder.Name);
        newBidder.Id = newId;
        List<Bidder> bidderList = bidderService.GetAllBidders();
        Assert.IsTrue(bidderList.Any(b => b.Id == newBidder.Id), "Bidder is not in the list after being added.");
        Bidder? checkBidder = bidderService.GetBidderById(newBidder.Id);
        Assert.IsNotNull(checkBidder, "Bidder is not found after being added.");
        Assert.AreEqual(checkBidder.Name, newBidder.Name, false, "Bidder data is different after being added.");

        // Update the bidder
        checkBidder.Name += " - edited";
        bool updateSuccess = bidderService.UpdateBidder(checkBidder);
        Assert.IsTrue(updateSuccess, "Unsuccessful response when updating the bidder.");
        Bidder? checkBidder2 = bidderService.GetBidderById(newBidder.Id);
        Assert.IsNotNull(checkBidder2, "Bidder is not found after being updated.");
        Assert.AreEqual(checkBidder2.Name, checkBidder.Name, "Bidder data is different after updating.");

        // Delete the bidder
        bool deleteSuccess = bidderService.DeleteBidder(checkBidder2.Id);
        Assert.IsTrue(deleteSuccess, "Unsuccessful response when deleting the bidder.");
        List<Bidder> bidderList2 = bidderService.GetAllBidders();
        Assert.IsFalse(bidderList2.Any(b => b.Id == checkBidder2.Id), "Bidder still in the list after being deleted.");
        Bidder? checkBidder3 = bidderService.GetBidderById(checkBidder2.Id);
        Assert.IsNull(checkBidder3, "Bidder is still found after being deleted.");
    }
}
