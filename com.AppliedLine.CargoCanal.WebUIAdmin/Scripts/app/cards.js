if (!__handlers) var __handlers = {};

var btns = document.getElementsByClassName('card-options-btn');
var handler = function () {
    toggleOptions(this);
}

if (btns.length > 0) {
    for (var i = 0; i < btns.length; i++) {
        if (i in __handlers) btns[i].removeEventListener('click', __handlers[i]);
        __handlers[i] = handler;
        btns[i].addEventListener('click', __handlers[i]);
    }
}

function toggleOptions(btn) {
    // let options = btn.parentNode;
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
