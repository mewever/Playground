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

// Disable the send button until connection is established.
document.getElementById("joinButton").disabled = true;

connection.on("ReceiveBid", function (message) {
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

connection.on("ReceiveSync", function (message) {
    myBidderId = message.bidderId;
    myBidderName = message.bidderName;
    items = message.items;
    redrawItems(items);
});

connection.start().then(function () {
    document.getElementById("joinButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("joinButton").addEventListener("click", function (event) {
    var name = document.getElementById("nameInput").value;
    if (myBidderId == -1) {
        connection.invoke("ConnectBidder", name).catch(function (err) {
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

function redrawItems() {
    console.log('redrawItems()');
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
    console.log('Sorting items...');
    displayItems = items.sort((a, b) => new Date(a.closingTime) - new Date(b.closingTime));
    console.log('Items sorted.');

    var el = document.getElementById("itemsDisplay");
    var itemsList = '<div class="row">';
    displayItems.forEach(item => {
        console.log(`Redrawing item ${item.itemId}`);
        var divId = `item_${item.id}`;
        var bidAmountId = `bidAmount_${item.itemId}`;
        var bidButtonId = `bidButton_${item.itemId}`;
        var closeTime = new Date(item.closingTime);
        var millisecondsBeforeClose = closeTime - Date.now();
        console.log(`closing in ${millisecondsBeforeClose} ms`);
        // Only display items that close in the future
        if (millisecondsBeforeClose > 0) {
            itemsList += `<div id="${divId}" class="col-12 col-sm-6 col-md-4 p-1">`
                + '<div class="border border-black p-1">'
                + `${item.name}<br/>`
                + `${item.description}<br/>`
                + `${item.currentBid}<br/>`
                + `Closes ${closeTime.toLocaleDateString()} ${closeTime.toLocaleTimeString()}`;
            itemsList += `<br/><input id="${bidAmountId}" value="${item.currentBid}" />`;
            itemsList += `<br/><button id="${bidButtonId}">Submit Bid</button>`;
            if (item.currentWinnerId == myBidderId) {
                itemsList += '<br\>WINNING';
            }
            itemsList += '</div>';
            itemsList += '</div>';

            // Set a timer to remove the item after it closes
            console.log(`Setting timer for item ${item.itemId}`);
            item.timer = setTimeout(() => {
                el = document.getElementById(divId).remove();
            }, millisecondsBeforeClose);
        }
    });
    itemsList += '</div>';
    console.log('Setting HTML');
    el.innerHTML = itemsList;

    // Set the bid button listeners (can't be done until the inner HTML has been set)
    displayItems.forEach(item => {
        // Set a listener for the submit bid button
        console.log(`Setting submit button listener for item ${item.itemId}`);
        var bidAmountId = `bidAmount_${item.itemId}`;
        var bidButtonId = `bidButton_${item.itemId}`;
        item.buttonListener = document.getElementById(bidButtonId).addEventListener("click", function (event) {
            var amount = document.getElementById(bidAmountId).value;
            var request = {
                bidderId: Number(myBidderId),
                itemId: Number(item.itemId),
                bidAmount: Number(amount)
            };
            console.log(JSON.stringify(request));
            connection.invoke("SubmitBid", request).catch(function (err) {
                return console.error(err.toString());
            });
            event.preventDefault();
        });
    });
}

function selectItem(itemId) {
    var idx = items.findIndex((i) => i.itemId == itemId);
    if (idx != -1) {
        var elSelectedItemId = document.getElementById("selectedItemId");
        elSelectedItemId.value = items[idx].itemId;
        var elSelectedItemName = document.getElementById("selectedItemName");
        elSelectedItemName.value = items[idx].name;
        var elBidAmount = document.getElementById("bidAmount");
        elBidAmount.value = items[idx].currentBid;
    }
}