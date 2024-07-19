// js.js
function addEventListener() {
    document.querySelectorAll('.hour').forEach(item => {
        item.addEventListener('mouseover', () => {
            item.style.transform = 'scale(1.02)';
        });
        item.addEventListener('mouseout', () => {
            item.style.transform = 'scale(1)';
        });
    });
}
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
