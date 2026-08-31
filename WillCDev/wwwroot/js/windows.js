function InitWindows() {

    // Draggable windows ========================================================================
    let dragsToInit = $('.draggable.start');
    dragsToInit.draggable();

    let draggables = $('.draggable');

    dragsToInit.toArray().forEach(element => {
        $(element).removeClass("start");
        $(element).addClass("init");
        var transitionAnimation = $(element).css('transition').split(' ')[0].replace('s', '');
        let animationTime = parseFloat(transitionAnimation) * 1000

        setTimeout(() => {
            let elmnt = $(element)[0].getBoundingClientRect();
            let width = elmnt.width;
            let height = elmnt.height;
            
            $(element).css('width', width);
            $(element).css('height', height);
            $(element).removeClass("init");
        }, animationTime);
    });

    draggables.on("mousedown", function () {
        draggables.toArray().forEach(element => {
            let zIndex = $(element).css("z-index") - 1;
            if (zIndex < 1)
                zIndex++;

            $(element).css("z-index", zIndex);
        });
        $(this).css("z-index", 1000);
    });
    // ==========================================================================================

    // Start Menu ===============================================================================
    let startMenu = $('.start-menu-frame.start');
    startMenu.removeClass("start");

    // ==========================================================================================

    
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

function CloseWindow(elementId) {
    let element = $("#" + elementId);
    element.css("width", 0);
    element.css("height", 0);
    element.addClass("start");
    var transitionAnimation = element.css("transition").split(" ")[0].replace("s", "");
    let animationTime = parseFloat(transitionAnimation) * 1000
    return animationTime;
}