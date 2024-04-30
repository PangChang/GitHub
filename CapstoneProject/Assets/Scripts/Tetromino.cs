using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z,
}



//custom attribute (helps displays data in editor)
[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells { get; private set; } //because this is a 2d game (can customize shapes in cells)
    public Vector2Int[,] wallKicks { get; private set; }

    public void Initialize() 
    {
        this.cells = Data.Cells[this.tetromino];//will look up data inside Data //pieces you designed
        this.wallKicks = Data.WallKicks[this.tetromino];
    }
}

