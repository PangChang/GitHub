using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameBoardTou : MonoBehaviour
{
    public Tilemap tilemapTou { get; private set; }
    public PieceTou activePieceTou { get; private set; }
    public TetrominoDataTou[] tetrominoTou;

    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public RectInt Bounds //built int RectInt function. will be the position of the board we want (the outside)
    {
        get
        {
            //Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    //calling functions
    private void Awake()
    {
        this.tilemapTou = GetComponentInChildren<Tilemap>();
        this.activePieceTou = GetComponentInChildren<PieceTou>();

        for (int i = 0; i < this.tetrominoTou.Length; i++)
        {
            this.tetrominoTou[i].Initialize();
        }
    }

    //reference for tilemap
    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoTou.Length);
        TetrominoDataTou dataTou = this.tetrominoTou[random];

        this.activePieceTou.Initialize(this, this.spawnPosition, dataTou);

        //gameOver, will stop the game when the pieces reaches the top
        if (IsValidPosition(this.activePieceTou, this.spawnPosition))
        {
            Set(this.activePieceTou);
        }
        else
        {
            GameOver();
        }
    }


    private void GameOver()
    {
        this.tilemapTou.ClearAllTiles();
        //send to the Game Over Screen

        SceneManager.LoadScene("Single End"); //Trying to end the game to get a scoreboard system

    }

    //function for setting certain piece
    public void Set(PieceTou pieceTou)
    {
        for (int i = 0; i < pieceTou.cellsTou.Length; i++)
        {
            Vector3Int tilePosition = pieceTou.cellsTou[i] + pieceTou.positionTou;
            this.tilemapTou.SetTile(tilePosition, pieceTou.dataTou.tileTou);
        }
    }

    public void Clear(PieceTou pieceTou)
    {
        for (int i = 0; i < pieceTou.cellsTou.Length; i++)
        {
            Vector3Int tilePosition = pieceTou.cellsTou[i] + pieceTou.positionTou;
            this.tilemapTou.SetTile(tilePosition, null);

        }
    }

    public bool IsValidPosition(PieceTou pieceTou, Vector3Int position) //to help code measure movement in Pieces
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < pieceTou.cellsTou.Length; i++)
        {
            Vector3Int tilePosition = pieceTou.cellsTou[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (this.tilemapTou.HasTile(tilePosition))
            {
                return false;
            }
        }
        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;



        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);//points are made when clearing row(s)
                Score.score++;
            }
            else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemapTou.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }

    private void LineClear(int row) //to clear lines and move the above blocks
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemapTou.SetTile(position, null);
        }
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemapTou.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemapTou.SetTile(position, above);
            }
            row++;
        }
    }
}
    
