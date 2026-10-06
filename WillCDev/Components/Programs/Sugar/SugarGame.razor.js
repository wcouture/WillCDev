let p5Instance = null;
let _sketch = null;

function initializeSugarGame(containerId) {
    const container = document.getElementById(containerId);
    if (container && !p5Instance) {
        p5Instance = new p5((sketch) => {
            _sketch = sketch;
            sketch.setup = InitGame;
            sketch.draw = UpdateGame;
        });
    }
}

function disposeSugarGame(containerId) {
    const container = document.getElementById(containerId);
    if (container) {
        container.innerHTML = "";
    }
    if (p5Instance) {
        p5Instance.remove();
        p5Instance = null;
    }
}

export { initializeSugarGame, disposeSugarGame };

let GAME_DATA = {
    container: null,
    width: 0,
    height: 0,
    levels: {
        1: false,
        2: false,
        3: false,
    },
    scene: "mainMenu"
}

function InitGame() {
    GAME_DATA.container = document.getElementById("game-container");
    GAME_DATA.width = GAME_DATA.container.clientWidth;
    GAME_DATA.height = GAME_DATA.container.clientHeight;
    const canvas = _sketch.createCanvas(GAME_DATA.width, GAME_DATA.height);
    canvas.parent(GAME_DATA.container);
    _sketch.background(200);
}

function UpdateGame() {
    ClearScreen();
    DrawScene();
}

function DrawScene() {
    switch (GAME_DATA.scene) {
        case "mainMenu":
            DrawMainMenu();
            break;
        // Add other scenes here as needed
    }
}

function ClearScreen() {
    _sketch.clear();
    _sketch.background(200);
}

function DrawMainMenu() {
    _sketch.textAlign(_sketch.CENTER, _sketch.CENTER);
    _sketch.textSize(32);
    _sketch.fill(0);
    _sketch.text("SUGAR", GAME_DATA.width / 2, GAME_DATA.height / 2);

    _sketch.textSize(16);
    _sketch.fill(0);
    _sketch.text("- Press any key to start -", GAME_DATA.width / 2, GAME_DATA.height / 2 + 40);
}


