function InitDraggables() {
    $(".draggable").draggable();

    $(".draggable").on("mousedown", function () {
        $(".draggable").toArray().forEach(element => {
            let zIndex = $(element).css("z-index");
            if (zIndex <= 1) {
                $(element).css("z-index", 1);
            } else {
                $(element).css("z-index", zIndex - 1);
            }
        });
        $(this).css("z-index", 1000);
    });
}

function BringToFront(elementId) {
    console.log("Bringing element to front:", elementId);
    $(".draggable").toArray().forEach(element => {
        let zIndex = $(element).css("z-index");
        if (zIndex <= 1) {
            $(element).css("z-index", 1);
        } else {
            $(element).css("z-index", zIndex - 1);
        }
    });
    $("#" + elementId).css("z-index", 1000);
}