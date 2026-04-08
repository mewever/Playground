"use strict";

class item {
    itemId = 0;
    name = '';
    description = '';
    currentBid = 0.00;
    currentWinnerId = 0;
    closingTime = new Date();
};

class syncMessage {
    bidderId = 0;
    bidderName = '';
    items = [];
}

let items = [];
let myBidderId = -1;
let myBidderName = '';
let displayItems = [];

var connection = new signalR.HubConnectionBuilder().withUrl("/bidHub").build();

const currencyFormatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
});

// Disable the join button until connection is established.
document.getElementById("joinButton").disabled = true;

// Function to receive information about a bid placed on one of the item
connection.on("ReceiveBid", function (message) {
    console.log(`Bid update received for item ID ${message.itemId} for ${message.currentBid}`)
    // Update the item that got a new bid
    var idx = items.findIndex((i) => i.itemId == message.itemId);
    if (idx == -1) {
        return;
    }
    items[idx].currentBid = message.currentBid;
    items[idx].currentWinnerId = message.currentWinnerId;
    items[idx].currentWinnerName = message.currentWinnerName;
    items[idx].closingTime = message.closingTime;
    redrawItems(items);
});

// Function to receive a full sync of items
connection.on("ReceiveSync", function (message) {
    console.log('Full sync received')
    myBidderId = message.bidderId;
    myBidderName = message.bidderName;
    items = message.items;
    redrawItems(items);
});

// Handle a completed connection
// - Enable the join button
connection.start().then(function () {
    document.getElementById("joinButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

// Listener for the join button
document.getElementById("joinButton").addEventListener("click", function (event) {
    var name = document.getElementById("nameInput").value;
    if (myBidderId == -1) {
        connection.invoke("ConnectBidder", name)
            .then(function () {
                // Disable the name input and hide the join button after they have joined
                document.getElementById("nameInput").disabled = true;
                document.getElementById("joinButton").hidden = true;
            })
            .catch(function (err) {
                return console.error(err.toString());
            });
        event.preventDefault();
    }
    else {
        connection.invoke("ReconnectBidder", myBidderId).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
});

// Redraw the list of items
// This will happen when a full sync is received
// or a bid is received
function redrawItems() {
    // Cancel existing timers
    for (item of displayItems) {
        if (item.timer) {
            console.log(`Clearing timeout for item ${item.itemId}`);
            clearTimeout(item.timer);
        }
        if (item.buttonListener) {
            console.log(`Clearing listener for item ${item.itemId}`);
            removeEventListener(item.buttonListener);
        }
    }

    // Set display items to items sorted by ascending closing time
    displayItems = items.sort((a, b) => new Date(a.closingTime) - new Date(b.closingTime));

    var el = document.getElementById("itemsDisplay");
    var itemsList = '<div class="row">';
    displayItems.forEach(item => {
        var divId = `item_${item.id}`;
        var bidAmountId = `bidAmount_${item.itemId}`;
        var bidButtonId = `bidButton_${item.itemId}`;
        var closeTime = new Date(item.closingTime);
        var millisecondsBeforeClose = closeTime - Date.now();
        // Only display items that close in the future
        if (millisecondsBeforeClose > 0) {
            itemsList += `<div id="${divId}" class="col-12 col-sm-6 col-md-4 p-1">`
                + '<div class="border border-black p-1">'
                + `<span class="fw-bold">${item.name}</span><br/>`
                + `${item.description}<br/>`
                + `Current Bid: ${currencyFormatter.format(item.currentBid)}<br/>`
                + `Closes ${closeTime.toLocaleDateString()} ${closeTime.toLocaleTimeString()}`;
            itemsList += '<div class="d-flex">';
            itemsList += `<input type="number" id="${bidAmountId}" class="form-control" value="${item.currentBid}" />`;
            itemsList += `<button id="${bidButtonId}" class="btn btn-primary ">Submit&nbsp;Bid</button>`;
            itemsList += '</div>';
            if (item.currentWinnerId == myBidderId) {
                itemsList += '<span class="bg-success text-white px-1">WINNING</span>';
            }
            itemsList += '</div>';
            itemsList += '</div>';

            // Set a timer to remove the item after it closes
            console.log(`Setting removal timer for item ${item.itemId}`);
            item.timer = setTimeout(() => {
                el = document.getElementById(divId);
                if (el) {
                    console.log(`Removing closed item ID ${item.itemId}`)
                    el.remove();
                }
            }, millisecondsBeforeClose);
        }
    });
    itemsList += '</div>';
    el.innerHTML = itemsList;

    // Set the bid button listeners (can't be done until the inner HTML has been set)
    displayItems.forEach(item => {
        // Use a try/catch here so the rest of the items get processed if this one gets an error
        // Errors might occur if the item is removed by its timer while this loop is occurring
        try {
            // Set a listener for the submit bid button
            var bidAmountId = `bidAmount_${item.itemId}`;
            var bidButtonId = `bidButton_${item.itemId}`;
            var el = document.getElementById(bidButtonId);
            if (el) {
                console.log(`Setting submit button listener for item ${item.itemId}`);
                item.buttonListener = el.addEventListener("click", function (event) {
                    var amountEl = document.getElementById(bidAmountId);
                    if (amountEl) {
                        var amount = amountEl.value;
                        var request = {
                            bidderId: Number(myBidderId),
                            itemId: Number(item.itemId),
                            bidAmount: Number(amount)
                        };
                        console.log(`Submitting bid for item ID ${item.itemId} for ${amount}`);
                        connection.invoke("SubmitBid", request).catch(function (err) {
                            return console.error(err.toString());
                        });
                    }
                    event.preventDefault();
                });
            }
        }
        catch (err) {
            // Log the error and continue on
            console.error(err.toString())
        }
    });
}