class WindowInstance {
    constructor(id, title, windowElement) {
        this.id = id;
        this.title = title;
        this.minimized = false;
        this.maximized = false;
        this.windowElement = windowElement;
    }
}

class WindowManager {
    constructor() {
        this.windows = [];
    }

    openWindow(id, title, shrunken = false) {
        try {
            // Get the window element using jQuery
            console.log("Opening window with id:", id, "and title:", title);
            const windowElement = $('#window-'+id);
            windowElement.draggable({
                handle: ".title-bar",
                cancel: ".window-body, .window-body *"
            }); // Make the window draggable from the title bar only
            windowElement.resizable({
                minHeight: 200, // Add a minimum height for resizing
                minWidth: 200 // Add a minimum width for resizing
            }); // Make the window resizable
            windowElement.on("mousedown", () => {
                this.bringWindowToFront(id);
            });
    
            if (shrunken) {
                console.log("Window is shrunken");
            }
    
            // Create a new WindowInstance and add it to the list of managed windows
            const windowInstance = new WindowInstance(id, title, windowElement);
            this.windows.push(windowInstance);
    
            // Animate the window from the "start" state to its initial size and position
            windowElement.removeClass("start");
            windowElement.addClass("init");
            if (shrunken) {
                windowElement.addClass("shrunken");
            }
            var transitionAnimation = windowElement.css('transition').split(' ')[0].replace('s', '');
            let animationTime = parseFloat(transitionAnimation) * 1000
    
            setTimeout(() => {
                let elmnt = windowElement[0].getBoundingClientRect();
                let width = elmnt.width;
                let height = elmnt.height;
                
                windowElement.css('width', width);
                windowElement.css('height', height);
                windowElement.removeClass("init");
            }, animationTime);

        } catch (error) {
            console.error("Error opening window:", error);
        }
    }

    closeWindow(windowId) {
        try {
            const windowInstance = this.windows.find(win => win.id === windowId);
            if (windowInstance === undefined) {
                return 0;
            }
            
            this.windows = this.windows.filter(win => win !== windowInstance);
            let windowElement = windowInstance.windowElement;
            windowElement.css("width", "0");
            windowElement.css("height", "0");
            windowElement.addClass("start");
            var transitionAnimation = windowElement.css('transition').split(' ')[0].replace('s', '');
            let animationTime = parseFloat(transitionAnimation) * 1000
            return animationTime;
        } catch (error) {
            console.error("Error closing window:", error);
        }
        return 0; // Return 0 if there was an error closing the window
    }

    disableDragging(windowId) {
        console.log("Disabling dragging for window with ID:", windowId);
        try {
            const windowInstance = this.windows.find(win => win.id === windowId);
            if (windowInstance) {
                console.log("Found window instance for ID:", windowId);
                $('#window-' + windowId).draggable("disable");
            }
        } catch (error) {
            console.error("Error pausing dragging for window:", error);
        }
    }

    enableDragging(windowId) {
        console.log("Enabling dragging for window with ID:", windowId);
        try {
            const windowInstance = this.windows.find(win => win.id === windowId);
            if (windowInstance) {
                console.log("Found window instance for ID:", windowId);
                $('#window-' + windowId).draggable("enable");
            }
        } catch (error) {
            console.error("Error enabling dragging for window:", error);
        }
    }

    bringWindowToFront(windowId) {
        try {
            const windowInstance = this.windows.find(win => win.id === windowId);
            if (windowInstance) {
                this.windows = this.windows.filter(win => win !== windowInstance);
                this.windows.push(windowInstance);
            }

            for (const win of this.windows) {
                win.windowElement.css("z-index", 1000 - (this.windows.length - this.windows.indexOf(win)));
            }
        } catch (error) {
            console.error("Error bringing window to front:", error);
        }
    }
}