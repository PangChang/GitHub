using UnityEngine;
using UnityEngine.Tilemaps;

public enum TetrominoTou
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z,
    Q,
    DotDotDot,
    Edge,
    Helmet,
    DotDot,
    Dot,
}



//custom attribute (helps displays data in editor)
[System.Serializable]
public struct TetrominoDataTou
{
    public TetrominoTou tetrominoTou;
    public Tile tileTou;
    public Vector2Int[] cellsTou { get; private set; } //because this is a 2d game (can customize shapes in cells)
    public Vector2Int[,] wallKicksTou { get; private set; }

    public void Initialize()
    {
        this.cellsTou = DataTou.Cells[this.tetrominoTou];//will look up data inside Data //pieces you designed
        this.wallKicksTou = DataTou.WallKicks[this.tetrominoTou];
    }
}


