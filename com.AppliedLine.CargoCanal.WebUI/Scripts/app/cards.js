if (!__handlers) var __handlers = {};
if (!__handlersCard) var __handlersCard = {};

var btns = document.getElementsByClassName('card-options-btn');
var cards = document.getElementsByClassName('card');
var handler = function () {
    toggleOptions(this);
}

var handlerCard = function () {
    toggleFlyouts(this);
}

/* default behaviour to slide cards in and out */
if (btns.length > 0) {
    for (var i = 0; i < btns.length; i++) {
        if (i in __handlers) btns[i].removeEventListener('click', __handlers[i]);
        __handlers[i] = handler;
        btns[i].addEventListener('click', __handlers[i]);
    }
}

/* handles card sliding in and out - fix for Safari */
if (cards.length > 0) {
    for (var j = 0; j < cards.length; j++) {
        if (j in __handlersCard) cards[j].removeEventListener('click', __handlersCard[j]);
        __handlersCard[j] = handlerCard;
        cards[j].addEventListener('click', __handlersCard[j]);
    }
}

function toggleOptions(btn) {
    let btnCardClass = btn.parentNode.getAttributeNode('class').value;
    closeCardsOpen();

    // open btnCardClass if state did not change
    if (btnCardClass === btn.parentNode.getAttributeNode('class').value) {
        btn.parentNode.getAttributeNode('class').value = 'card-options card-options-open';
    }
}

function closeCardsOpen() {
    let cardsOpen = document.getElementsByClassName('card-options card-options-open');

    if (cardsOpen.length > 0) {
        cardsOpen[0].getAttributeNode('class').value = 'card-options card-options-close';
    }
}

function toggleFlyouts(card) {
    let btn = card.getElementsByClassName('card-options-btn');

    closeFlyoutsOpen();

    if (btn.length > 0) {
        // add flyout to the class
        if (btn[0].getAttributeNode('class').value !== 'card-options-btn card-flyout')
            btn[0].getAttributeNode('class').value = 'card-options-btn card-flyout';
    }
}

function closeFlyoutsOpen() {
    let flyoutsOpen = document.getElementsByClassName('card-options-btn card-flyout');

    if (flyoutsOpen.length > 0) {
        flyoutsOpen[0].getAttributeNode('class').value = 'card-options-btn';
    }
};