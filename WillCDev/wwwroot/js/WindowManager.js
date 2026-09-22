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
        // Get the window element using jQuery
        console.log("Opening window with id:", id, "and title:", title);
        const windowElement = $('#window-'+id);
        windowElement.draggable(); // Make the window draggable
        windowElement.resizable(); // Make the window resizable
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
    }

    closeWindow(windowId) {
        const windowInstance = this.windows.find(win => win.id === windowId);
        if (windowInstance) {
            this.windows = this.windows.filter(win => win !== windowInstance);
        }

        let windowElement = windowInstance.windowElement;
        windowElement.css("width", "0");
        windowElement.css("height", "0");
        windowElement.addClass("start");
        var transitionAnimation = windowElement.css('transition').split(' ')[0].replace('s', '');
        let animationTime = parseFloat(transitionAnimation) * 1000
        return animationTime;
    }

    bringWindowToFront(windowId) {
        const windowInstance = this.windows.find(win => win.id === windowId);
        if (windowInstance) {
            this.windows = this.windows.filter(win => win !== windowInstance);
            this.windows.push(windowInstance);
        }

        for (const win of this.windows) {
            win.windowElement.css("z-index", 1000 - (this.windows.length - this.windows.indexOf(win)));
        }
    }
}