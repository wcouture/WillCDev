function InitDraggables() {
    let draggables = $('.draggable');

    draggables.draggable();

    draggables.toArray().forEach(element => {
        let elmnt = $(element)[0].getBoundingClientRect();
        let width = elmnt.width;
        let height = elmnt.height;

        $(element).removeClass("init");
        $(element).css('width', width);
        $(element).css('height', height);
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