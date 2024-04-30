using System.Collections.Generic;
using UnityEngine;

public static class DataTou //can access this data from everywhere
{
    public static readonly float cosTou = Mathf.Cos(Mathf.PI / 2f);
    public static readonly float sinTou = Mathf.Sin(Mathf.PI / 2f);
    public static readonly float[] RotationMatrixTou = new float[] { cosTou, sinTou, -sinTou, cosTou };


    //Everything shape is set up through coding, but can manually set up in unity settings
    //fix static data if wanting to make custom shapes
    public static readonly Dictionary<TetrominoTou, Vector2Int[]> Cells = new Dictionary<TetrominoTou, Vector2Int[]>() //dictionary of my shapes
    {
        {TetrominoTou.I, new Vector2Int[] { new Vector2Int( -1, 1), new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 2, 1) } },
        {TetrominoTou.J, new Vector2Int[] { new Vector2Int( -1, 1), new Vector2Int( -1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        {TetrominoTou.L, new Vector2Int[] { new Vector2Int( 1, 1), new Vector2Int( -1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        {TetrominoTou.O, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        {TetrominoTou.S, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( -1, 0), new Vector2Int( 0, 0) } },
        {TetrominoTou.T, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( -1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        {TetrominoTou.Z, new Vector2Int[] { new Vector2Int( -1, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        {TetrominoTou.Q, new Vector2Int[] { new Vector2Int( -1, 1), new Vector2Int( -1, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } }, //similar to an L, but missing above the handle
        {TetrominoTou.DotDotDot, new Vector2Int[] { new Vector2Int( -1, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 1), new Vector2Int( 1, 1) } },//smaller line
        {TetrominoTou.Edge, new Vector2Int[] { new Vector2Int( -1, 1), new Vector2Int( -1, 0), new Vector2Int( 0, 0), new Vector2Int( 0, 0) } },//Edge of blocks
        {TetrominoTou.Helmet, new Vector2Int[] { new Vector2Int( -1, 0), new Vector2Int( -1, 0), new Vector2Int( 0, 1), new Vector2Int( 1, 0) } },//surrounds a block
        {TetrominoTou.DotDot, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 0), new Vector2Int( 0, 0) } },//even smaller line
        {TetrominoTou.Dot, new Vector2Int[] { new Vector2Int( 0, 0), new Vector2Int( 0, 0), new Vector2Int( 0, 0), new Vector2Int( 0, 0) } }//just a dot

    };



    //to prevent the shapes from going through the boarders (for the rotating portion)
    private static readonly Vector2Int[,] WallKicksI = new Vector2Int[,]
    {
        { new Vector2Int( 0, 0), new Vector2Int( -2, 0), new Vector2Int( 1, 0), new Vector2Int( -2, -1), new Vector2Int( 1, 2) },
        { new Vector2Int( 0, 0), new Vector2Int( 2, 0), new Vector2Int( -1, 0), new Vector2Int( 2, 1), new Vector2Int( -1, -2) },
        { new Vector2Int( 0, 0), new Vector2Int( -1, 0), new Vector2Int( 2, 0), new Vector2Int( -1, 2), new Vector2Int( 2, -1) },
        { new Vector2Int( 0, 0), new Vector2Int( 1, 0), new Vector2Int( -2, 0), new Vector2Int( 1, -2), new Vector2Int( -2, 1) },
        { new Vector2Int( 0, 0), new Vector2Int( 2, 0), new Vector2Int( -1, 0), new Vector2Int( 2, 1), new Vector2Int( -1, -2) },
        { new Vector2Int( 0, 0), new Vector2Int( -2, 0), new Vector2Int( 1, 0), new Vector2Int( -2, -1), new Vector2Int( 1, 2) },
        { new Vector2Int( 0, 0), new Vector2Int( 1, 0), new Vector2Int( -2, 0), new Vector2Int( 1, -2), new Vector2Int( -2, 1) },
        { new Vector2Int( 0, 0), new Vector2Int( -1, 0), new Vector2Int( 2, 0), new Vector2Int( -1, 2), new Vector2Int( 2, -1) },
    };

    private static readonly Vector2Int[,] WallKicksJLOSTZ = new Vector2Int[,]
    {
        { new Vector2Int( 0, 0), new Vector2Int( -1, 0), new Vector2Int( -1, 1), new Vector2Int( 0, -2), new Vector2Int( -1, -2) },
        { new Vector2Int( 0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, -1), new Vector2Int( 0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int( 0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, -1), new Vector2Int( 0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int( 0, 0), new Vector2Int( -1, 0), new Vector2Int( -1, 1), new Vector2Int( 0, -2), new Vector2Int( -1, -2) },
        { new Vector2Int( 0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int( 0, -2), new Vector2Int( 1, -2) },
        { new Vector2Int( 0, 0), new Vector2Int( -1, 0), new Vector2Int( -1, -1), new Vector2Int( 0, 2), new Vector2Int( -1, 2) },
        { new Vector2Int( 0, 0), new Vector2Int( -1, 0), new Vector2Int( -1, -1), new Vector2Int( 0, 2), new Vector2Int( -1, 2) },
        { new Vector2Int( 0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int( 0, -2), new Vector2Int( 1, -2) },
    };

    public static readonly Dictionary<TetrominoTou, Vector2Int[,]> WallKicks = new Dictionary<TetrominoTou, Vector2Int[,]>()
    {
        {TetrominoTou.I, WallKicksI},
        {TetrominoTou.J, WallKicksJLOSTZ},
        {TetrominoTou.L, WallKicksJLOSTZ},
        {TetrominoTou.O, WallKicksJLOSTZ},
        {TetrominoTou.S, WallKicksJLOSTZ},
        {TetrominoTou.T, WallKicksJLOSTZ},
        {TetrominoTou.Z, WallKicksJLOSTZ},
        {TetrominoTou.Q, WallKicksJLOSTZ},
        {TetrominoTou.DotDotDot, WallKicksJLOSTZ},
        {TetrominoTou.Edge, WallKicksI}, //to give a more proper wall kick
        {TetrominoTou.Helmet, WallKicksJLOSTZ},
        {TetrominoTou.DotDot, WallKicksJLOSTZ},
        {TetrominoTou.Dot, WallKicksJLOSTZ},
    };



}
