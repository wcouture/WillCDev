let p5Instance = null;
let _sketch = null;

export function initializeSugarGame(containerId) {
    const container = document.getElementById(containerId);
    if (container && !p5Instance) {
        p5Instance = new p5((sketch) => {
            _sketch = sketch;
            sketch.setup = InitGame;
            sketch.draw = UpdateGame;
            sketch.keyPressed = () => {
                HandleKeyPress(sketch.keyCode);
            }
        });
    }
}

export function disposeSugarGame(containerId) {
    const container = document.getElementById(containerId);
    if (container) {
        container.innerHTML = "";
    }
    if (p5Instance) {
        p5Instance.remove();
        p5Instance = null;
    }
}

let GAME_DATA = {
    container: null,
    width: 0,
    height: 0,
    levels: {
        1: false,
        2: false,
        3: false,
    },
    scene: "mainMenu",
    cellSize: 2,
    gridData: []
}

function InitGame() {
    GAME_DATA.container = document.getElementById("game-container");
    GAME_DATA.width = GAME_DATA.container.clientWidth;
    GAME_DATA.height = GAME_DATA.container.clientHeight;
    const canvas = _sketch.createCanvas(GAME_DATA.width, GAME_DATA.height);
    canvas.parent(GAME_DATA.container);
    _sketch.background(200);


    // Initialize the grid data
    GAME_DATA.gridData = Array(GAME_DATA.height / GAME_DATA.cellSize).fill().map(() => Array(GAME_DATA.width / GAME_DATA.cellSize).fill(0));
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
        case "game":
            DrawGame();
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

function DrawGame() {
    // Implement the game drawing logic here
    let gridX = Math.floor(_sketch.mouseX / GAME_DATA.cellSize);
    let gridY = Math.floor(_sketch.mouseY / GAME_DATA.cellSize);

    _sketch.square(gridX * GAME_DATA.cellSize, gridY * GAME_DATA.cellSize, GAME_DATA.cellSize);

    // Check if the mouse is within the canvas bounds
    let mouseWithinBounds = gridX >= 0 && gridX < GAME_DATA.width / GAME_DATA.cellSize && gridY >= 0 && gridY < GAME_DATA.height / GAME_DATA.cellSize;

    if (_sketch.mouseIsPressed && mouseWithinBounds) {
        GAME_DATA.gridData[gridY][gridX] = 1;
    }

    // Draw the grid
    DrawGrid();

    UpdateGrid();
}

function DrawGrid() {
    for (let y = 0; y < GAME_DATA.gridData.length; y++) {
        for (let x = 0; x < GAME_DATA.gridData[y].length; x++) {
            if (GAME_DATA.gridData[y][x] === 1) {
                _sketch.square(x * GAME_DATA.cellSize, y * GAME_DATA.cellSize, GAME_DATA.cellSize);
            }
        }
    }
}

function UpdateGrid() {
    // Create a copy of the current grid to store the next state
    let newGridData = GAME_DATA.gridData.map(row => row.slice());

    for (let y = 0; y < GAME_DATA.gridData.length; y++) {
        for (let x = 0; x < GAME_DATA.gridData[y].length; x++) {
            if (GAME_DATA.gridData[y][x] === 1) {
                let belowOpen = (y + 1 < GAME_DATA.gridData.length && GAME_DATA.gridData[y + 1][x] === 0);
                let leftDiagonalOpen = (y + 1 < GAME_DATA.gridData.length && x - 1 >= 0 && GAME_DATA.gridData[y + 1][x - 1] === 0);
                let rightDiagonalOpen = (y + 1 < GAME_DATA.gridData.length && x + 1 < GAME_DATA.gridData[y].length && GAME_DATA.gridData[y + 1][x + 1] === 0);

                if (belowOpen) {
                    let random = Math.random();
                    // random side-to-side movement when falling
                    if (random < 0.2 && x + 1 < GAME_DATA.gridData[y].length) {
                        newGridData[y + 1][x + 1] = 1;
                        newGridData[y][x] = 0;
                    } else if (random < 0.4 && x - 1 >= 0) {
                        newGridData[y + 1][x - 1] = 1;
                        newGridData[y][x] = 0;
                    } else {
                        newGridData[y + 1][x] = 1;
                        newGridData[y][x] = 0;
                    }
                } else if (leftDiagonalOpen) {
                    newGridData[y + 1][x - 1] = 1;
                    newGridData[y][x] = 0;
                } else if (rightDiagonalOpen) {
                    newGridData[y + 1][x + 1] = 1;
                    newGridData[y][x] = 0;
                }
            }
        }
    }

    GAME_DATA.gridData = newGridData;
}


// Handle key press events based on the current scene ======================================================
function HandleKeyPress(keyCode) {
    switch (GAME_DATA.scene) {
        case "mainMenu":
            MainMenuKeyPress(keyCode);
            break;
        // Add other scenes here as needed
    }
}

function MainMenuKeyPress(keyCode) {
    if (keyCode === 32) {
        GAME_DATA.scene = "game";
    }
}
