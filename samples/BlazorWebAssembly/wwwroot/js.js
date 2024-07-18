// js/js.js

function addClass(elementId, className) {
    var element = document.getElementById(elementId);
    if (element) {
        element.classList.add(className);
    } else {
        console.error(`Element with id '${elementId}' not found.`);
    }
}

function removeClass(elementId, className) {
    var element = document.getElementById(elementId);
    if (element) {
        element.classList.remove(className);
    } else {
        console.error(`Element with id '${elementId}' not found.`);
    }
}
