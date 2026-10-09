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

const GridCellType = {
    SCENE_BARRIER: 0,
    SALT: 1,
    OUT_OF_BOUNDS: 2,
    COLOR_CHANGER: 3
};

class GridCell {
    constructor(x, y, type, color) {
        this.x = x;
        this.y = y;
        this.type = type;
        this.color = color;
    }
}

const OUT_OF_BOUNDS_CELL = new GridCell(-1, -1, GridCellType.OUT_OF_BOUNDS, `#FF00FF`);

class GridCellContainer {
    constructor() {
        this.cells = [];
    }

    addCell(cell) {
        this.cells.push(cell);
    }
    
    removeCell(cell) {
        const index = this.cells.indexOf(cell);
        if (index !== -1) {
            this.cells.splice(index, 1);
        }
    }

    getCells() {
        return this.cells;
    }
}

let GAME_DATA = {
    loaded: false,
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
    gridData: [],
    actionCellData: []
}

function InitGame() {
    GAME_DATA.container = document.getElementById("game-container");
    GAME_DATA.width = GAME_DATA.container.clientWidth;
    GAME_DATA.height = GAME_DATA.container.clientHeight;
    const canvas = _sketch.createCanvas(GAME_DATA.width, GAME_DATA.height);
    canvas.parent(GAME_DATA.container);
    _sketch.background(200);
    _sketch.strokeWeight(1);

    // Initialize the grid data
    GAME_DATA.gridData = Array(GAME_DATA.height / GAME_DATA.cellSize).fill().map(() => Array(GAME_DATA.width / GAME_DATA.cellSize).fill(undefined));
    GAME_DATA.actionCellData = Array(GAME_DATA.height / GAME_DATA.cellSize).fill().map(() => Array(GAME_DATA.width / GAME_DATA.cellSize).fill(undefined));

    // Load level png
    const levelImage = new Image();
    levelImage.src = "images/Salt/SG_Level_0.png";
    levelImage.onload = () => {
        LoadMapData(levelImage);
        GAME_DATA.loaded = true;
    };
}

function LoadMapData(levelImage) {
    console.log("loading map...")
    const canvas = document.createElement("canvas");
    canvas.width = levelImage.width;
    canvas.height = levelImage.height;
    const context = canvas.getContext("2d");
    context.drawImage(levelImage, 0, 0);
    const imageData = context.getImageData(0, 0, levelImage.width, levelImage.height).data;

    console.log(`Grid data initialized:  ${JSON.stringify(GAME_DATA.gridData.length)} rows by ${JSON.stringify(GAME_DATA.gridData[0].length)} columns`);
    for (let y = 0; y < GAME_DATA.gridData.length; y++) {
        for (let x = 0; x < GAME_DATA.gridData[y].length; x++) {
            const pixelIndex = x * 4 + y * GAME_DATA.gridData[0].length * 4;
            const r = imageData[pixelIndex];
            const g = imageData[pixelIndex + 1];
            const b = imageData[pixelIndex + 2];
            const a = imageData[pixelIndex + 3];

            const hasValue = a > 180
            const isBlack = r === 0 && g === 0 && b === 0;
            if (!hasValue)
                continue;

            if (isBlack) {
                console.log("Adding black cell at", x, y);
                GAME_DATA.gridData[y][x] = new GridCell(x, y, GridCellType.SCENE_BARRIER, `#000000`);
            } 
            else {
                GAME_DATA.actionCellData[y][x] = new GridCell(x, y, GridCellType.COLOR_CHANGER, `rgba(${r},${g},${b},1)`);
            }
        }
    }
}

function UpdateGame() {
    ClearScreen();
    DrawScene();
}

function DrawScene() {
    if (!GAME_DATA.loaded) {
        DrawLoadingScreen();
    }

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

function DrawLoadingScreen() {
    _sketch.textAlign(_sketch.CENTER, _sketch.CENTER);
    _sketch.textSize(32);
    _sketch.fill(0);
    _sketch.text("Loading...", GAME_DATA.width / 2, GAME_DATA.height / 2);
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
        GAME_DATA.gridData[gridY][gridX] = new GridCell(gridX, gridY, GridCellType.SALT, `#fae098`);
    }

    // Draw the grid
    DrawGrid();

    UpdateGrid();
}

function DrawGrid() {
    for (let y = 0; y < GAME_DATA.gridData.length; y++) {
        for (let x = 0; x < GAME_DATA.gridData[y].length; x++) {
            const cell = GAME_DATA.gridData[y][x] ? GAME_DATA.gridData[y][x] : GAME_DATA.actionCellData[y][x];
            if (cell instanceof GridCell) {
                _sketch.push()
                _sketch.fill(cell.color);
                _sketch.stroke(cell.color);
                _sketch.square(x * GAME_DATA.cellSize, y * GAME_DATA.cellSize, GAME_DATA.cellSize);
                _sketch.pop()
            }
        }
    }
}

function UpdateGrid() {
    // Create a copy of the current grid to store the next state
    let newGridData = GAME_DATA.gridData.map(row => row.map(cell => cell ? new GridCell(cell.x, cell.y, cell.type, cell.color) : undefined));

    
    for (let y = 0; y < GAME_DATA.gridData.length; y++) {
        for (let x = 0; x < GAME_DATA.gridData[y].length; x++) {
            const cell = GAME_DATA.gridData[y][x];
            if (cell instanceof GridCell && cell.type === GridCellType.SALT) {
                const actionCell = GAME_DATA.actionCellData[y][x];
                if (actionCell !== undefined) {
                    // Perform some action with the actionCell if needed
                    if (actionCell.type === GridCellType.COLOR_CHANGER) {
                        cell.color = actionCell.color;
                    }
                }
                
                let leftNeighbors = (x - 1 >= 0) ? GAME_DATA.gridData[y][x - 1] : OUT_OF_BOUNDS_CELL;
                let rightNeighbors = (x + 1 < GAME_DATA.gridData[y].length) ? GAME_DATA.gridData[y][x + 1] : OUT_OF_BOUNDS_CELL;
                let leftDiagonalNeighbors = (y + 1 < GAME_DATA.gridData.length && x - 1 >= 0) ? GAME_DATA.gridData[y + 1][x - 1] : OUT_OF_BOUNDS_CELL;
                let rightDiagonalNeighbors = (y + 1 < GAME_DATA.gridData.length && x + 1 < GAME_DATA.gridData[y].length) ? GAME_DATA.gridData[y + 1][x + 1] : OUT_OF_BOUNDS_CELL;
                let belowNeighbors = (y + 1 < GAME_DATA.gridData.length) ? GAME_DATA.gridData[y + 1][x] : OUT_OF_BOUNDS_CELL;

                const belowOpen = belowNeighbors ? false : true;
                const leftDiagonalOpen = leftDiagonalNeighbors ? false : true;
                const rightDiagonalOpen = rightDiagonalNeighbors ? false : true;
                const leftOpen = leftNeighbors ? false : true;
                const rightOpen = rightNeighbors ? false : true;

                const moveCell = (fromX, fromY, toX, toY) => {
                    try {
                        newGridData[toY][toX] = cell;
                        newGridData[fromY][fromX] = undefined;
                        cell.x = toX;
                        cell.y = toY;

                    } catch (error) {
                        console.error("Error moving cell:", error);
                        console.log("Failed to move cell from", fromX, fromY, "to", toX, toY);
                    }
                };

                if (belowOpen) {
                    let random = Math.random();

                    // random side-to-side movement when falling
                    if (random < 0.2 && rightOpen && rightDiagonalOpen) {
                        console.log("moving below diagonal to the right");
                        moveCell(x, y, x + 1, y + 1);
                    } else if (random < 0.4 && leftOpen && leftDiagonalOpen) {
                        console.log("moving below diagonal to the left");
                        moveCell(x, y, x - 1, y + 1);
                    } else {
                        console.log("moving directly below");
                        moveCell(x, y, x, y + 1);
                    }
                } else if (leftOpen && leftDiagonalOpen) {
                    console.log("moving left and down");
                    moveCell(x, y, x - 1, y + 1);
                } else if (rightOpen && rightDiagonalOpen) {
                    console.log("moving right and down");
                    moveCell(x, y, x + 1, y + 1);
                }
                else {
                    console.log("cell cannot move");
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
