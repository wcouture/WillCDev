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

let p5Instance = null;
let _sketch = null;

const OUT_OF_BOUNDS_CELL = new GridCell(-1, -1, GridCellType.OUT_OF_BOUNDS, `#FF00FF`);

let GAME_DATA = {
    loading: true,
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
    gridData: undefined,
    actionCellData: undefined,
    trackedParticles: [],
    gridImage: undefined,
}

export function initializeSaltGame(containerId) {
    const container = document.getElementById(containerId);
    if (container && !p5Instance) {
        p5Instance = new p5((sketch) => {
            _sketch = sketch;
            sketch.setup = InitApplication;
            sketch.draw = UpdateGame;
            sketch.keyPressed = () => {
                HandleKeyPress(sketch.keyCode);
            }
        });
    }
}

export function disposeSaltGame(containerId) {
    const container = document.getElementById(containerId);
    if (container) {
        container.innerHTML = "";
    }
    if (p5Instance) {
        p5Instance.remove();
        p5Instance = null;
    }
}

async function InitApplication() {
    GAME_DATA.container = document.getElementById("game-container");
    GAME_DATA.width = GAME_DATA.container.clientWidth;
    GAME_DATA.height = GAME_DATA.container.clientHeight;
    const canvas = _sketch.createCanvas(GAME_DATA.width, GAME_DATA.height);
    canvas.parent(GAME_DATA.container);
    _sketch.background(200);
    _sketch.strokeWeight(0);

    // Load custom font for the game
    let font = await _sketch.loadFont('fonts/Vibes-Regular.ttf')
    _sketch.textFont(font);

    GAME_DATA.loading = false;
}

function InitializeGrid() {
    GAME_DATA.loading = true;

    GAME_DATA.gridData = Array(GAME_DATA.height / GAME_DATA.cellSize).fill().map(() => Array(GAME_DATA.width / GAME_DATA.cellSize).fill(undefined));
    GAME_DATA.actionCellData = Array(GAME_DATA.height / GAME_DATA.cellSize).fill().map(() => Array(GAME_DATA.width / GAME_DATA.cellSize).fill(undefined));

    // Load level png
    const levelImage = new Image();
    levelImage.src = "images/Salt/SG_Level_0.png";
    levelImage.onload = () => {
        LoadMapData(levelImage);
        GAME_DATA.loading = false;
    };
}

// Loads map data from a given level image into the game's grid data
function LoadMapData(levelImage) {
    console.log(`Loading map data from image: ${levelImage.src}`);
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

            const hasValue = a > 200
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

// Top-level game update loop handler
function UpdateGame() {
    ClearScreen();
    DrawScene();
}

// Higher-level update loop handler, delegates to specific scene handlers
function DrawScene() {
    if (GAME_DATA.loading) {
        DrawLoadingScreen();
        return;
    }

    switch (GAME_DATA.scene) {
        case "mainMenu":
            DrawMainMenu();
            break;
        case "game":
            RunGame();
            break;
        // Add other scenes here as needed
    }
}

// Clears the entire canvas
function ClearScreen() {
    _sketch.clear();
    _sketch.background(200);
}

// Loading screen handler
function DrawLoadingScreen() {
    _sketch.textAlign(_sketch.CENTER, _sketch.CENTER);
    _sketch.textSize(32);
    _sketch.fill(0);
    _sketch.text("Loading...", GAME_DATA.width / 2, GAME_DATA.height / 2);
}

// Main menu scene update loop handler
function DrawMainMenu() {
    _sketch.textAlign(_sketch.CENTER, _sketch.CENTER);
    _sketch.textSize(32);
    _sketch.fill(0);
    _sketch.text("SUGAR", GAME_DATA.width / 2, GAME_DATA.height / 2);

    _sketch.textSize(16);
    _sketch.fill(0);
    _sketch.text("- Press any key to start -", GAME_DATA.width / 2, GAME_DATA.height / 2 + 40);
}

// Spawns a salt particle at the specified grid coordinates with an optional color
function SpawnSalt(gridX, gridY, color = `#ffffff`) {
    GAME_DATA.gridData[gridY][gridX] = new GridCell(gridX, gridY, GridCellType.SALT, color);
    GAME_DATA.trackedParticles.push({ x: gridX, y: gridY, type: GridCellType.SALT });
}

// Game scene update loop handler
function RunGame() {
    if (!GAME_DATA.gridData) {
        InitializeGrid();
        return;
    }

    // Implement the game drawing logic here
    let gridX = Math.floor(_sketch.mouseX / GAME_DATA.cellSize);
    let gridY = Math.floor(_sketch.mouseY / GAME_DATA.cellSize);

    _sketch.square(gridX * GAME_DATA.cellSize, gridY * GAME_DATA.cellSize, GAME_DATA.cellSize);

    // Check if the mouse is within the canvas bounds
    let mouseWithinBounds = gridX >= 0 && gridX < GAME_DATA.width / GAME_DATA.cellSize && gridY >= 0 && gridY < GAME_DATA.height / GAME_DATA.cellSize;

    // Spawn sand at the current mouse position if the mouse is pressed and within bounds
    if (_sketch.mouseIsPressed && mouseWithinBounds) {
        SpawnSalt(gridX, gridY);
    }

    // Draw the grid
    DrawGrid();

    UpdateGrid();
}

// Game scene drawing and updating functions
function DrawGrid() {
    for (let y = 0; y < GAME_DATA.gridData.length; y++) {
        for (let x = 0; x < GAME_DATA.gridData[y].length; x++) {
            let cell = GAME_DATA.gridData[y][x]
            if (!cell) {
                cell = GAME_DATA.actionCellData[y][x];
            }

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

function UpdateParticle(x, y, newGridData) {
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
                return {newX: toX, newY: toY};
    
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
                return moveCell(x, y, x + 1, y + 1);
            } else if (random < 0.4 && leftOpen && leftDiagonalOpen) {
                console.log("moving below diagonal to the left");
                return moveCell(x, y, x - 1, y + 1);
            } else {
                console.log("moving directly below");
                return moveCell(x, y, x, y + 1);
            }
        } else if (leftOpen && leftDiagonalOpen) {
            console.log("moving left and down");
            return moveCell(x, y, x - 1, y + 1);
        } else if (rightOpen && rightDiagonalOpen) {
            console.log("moving right and down");
            return moveCell(x, y, x + 1, y + 1);
        }
        else {
            console.log("cell cannot move");
            return {newX: x, newY: y};
        }
    }
}

function UpdateGrid() {
    // Create a copy of the current grid to store the next state
    let newGridData = GAME_DATA.gridData.map(row => row.map(cell => cell ? new GridCell(cell.x, cell.y, cell.type, cell.color) : undefined));
    
    for (let p = 0; p < GAME_DATA.trackedParticles.length; p++) {
        const particle = GAME_DATA.trackedParticles[p];
        const newPosition = UpdateParticle(particle.x, particle.y, newGridData);
        particle.x = newPosition.newX;
        particle.y = newPosition.newY;
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
