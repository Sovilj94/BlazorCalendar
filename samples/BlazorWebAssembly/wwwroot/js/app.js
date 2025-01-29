window.HelloWorld = () => {
    console.log('Hello World from app.js');
}

window.AddClass = () => {   
    console.log("Add class");
}

window.GetBoundingClientRect = function (elementId) {
    console.log(`Trying to get bounding client rect for element with id: ${elementId}`);
    var element = document.getElementById(elementId);
    if (element) {
        var rect = element.getBoundingClientRect();
        console.log('Bounding client rect:', rect);
        return rect;
    } else {
        console.error(`Element with id '${elementId}' not found.`);
        return null;
    }
}