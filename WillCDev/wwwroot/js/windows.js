const WINDOW_MANAGER = new WindowManager();

function OpenWindow(elementId, title, shrunken = false) {
    WINDOW_MANAGER.openWindow(elementId, title, shrunken);
}

function CloseWindow(elementId) {
    return WINDOW_MANAGER.closeWindow(elementId);
}

function FocusWindow(elementId) {
    WINDOW_MANAGER.bringWindowToFront(elementId);
}

function BringToFront(elementId) {
    $(".draggable").toArray().forEach(element => {
        let zIndex = $(element).css("z-index") - 1;
        if (zIndex < 1)
            zIndex++;

        $(element).css("z-index", zIndex);
    });

    $("#" + elementId).css("z-index", 1000);
}

function FocusElement(elementId) {
    console.log("focus: ", elementId);
    $("#" + elementId).focus();
}

function ShrinkWindow(elementId) {
    let element = $("#" + elementId);
    
    var transitionAnimation = $(element).css('transition').split(' ')[0].replace('s', '');
    let animationTime = parseFloat(transitionAnimation) * 1000

    setTimeout(() => {
        element.css("height", "min-content");
    
        let width = element[0].getBoundingClientRect().width;
        let height = element[0].getBoundingClientRect().height;
    
        element.css("width", width);
        element.css("height", height);
    }, animationTime);
}