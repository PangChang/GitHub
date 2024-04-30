using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GameBoardTwo : MonoBehaviour
{
    public GameObject gameBoardTwo;
    public Tilemap tilemap { get; private set; }
    public PieceTwo activePiece { get; private set; }
    public TetrominoData[] tetrominoes;

    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);

/// ///////////////////////////////////////////////////////////////////////////////////////


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
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<PieceTwo>();
        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }

    //reference for tilemap
    private void Start()
    {

        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, this.spawnPosition, data);

        //gameOver, will stop the game when the pieces reaches the top
        if (IsValidPosition(this.activePiece, this.spawnPosition))
        {
            Set(this.activePiece);
        }
        else
        {
            Score.TwoInt = false;//will show the player has died
            GameOver();
        }
    }

    private void GameOver()
    {
        this.tilemap.ClearAllTiles();
        //send to the Game Over Screen


        if (Score.playerTwoScore < Score.playerOneScore && Score.OneInt == false)
        {
            SceneManager.LoadScene("P1 End"); //Trying to end the game to get a scoreboard system
        }
        else if (Score.playerOneScore <= Score.playerTwoScore)
        {
            gameBoardTwo.SetActive(false);//will disable my inputs
        }

        if (Score.playerOneScore < Score.playerTwoScore && Score.OneInt == false)
        {
            SceneManager.LoadScene("P2 End"); //Trying to end the game to get a scoreboard system
        }
        else if (Score.playerTwoScore <= Score.playerOneScore)
        {
            gameBoardTwo.SetActive(false);//will disable my inputs
        }

        if (Score.TwoInt == false && Score.OneInt == false && Score.playerOneScore == Score.playerTwoScore)
        {
            SceneManager.LoadScene("Draw End");//Draw scene, if tied
        }

    }

    //function for setting certain piece
    public void Set(PieceTwo pieceTwo)
    {
        for (int i = 0; i < pieceTwo.cells.Length; i++)
        {
            Vector3Int tilePosition = pieceTwo.cells[i] + pieceTwo.position;
            this.tilemap.SetTile(tilePosition, pieceTwo.data.tile);
        }
    }

    public void Clear(PieceTwo pieceTwo)
    {
        for (int i = 0; i < pieceTwo.cells.Length; i++)
        {
            Vector3Int tilePosition = pieceTwo.cells[i] + pieceTwo.position;
            this.tilemap.SetTile(tilePosition, null);

        }
    }

    public bool IsValidPosition(PieceTwo pieceTwo, Vector3Int position) //to help code measure movement in Pieces
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < pieceTwo.cells.Length; i++)
        {
            Vector3Int tilePosition = pieceTwo.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition))
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
                Score.playerTwoScore++;
            }//score
            else
            {
                row++;
            }
        }
    } //piece

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position))
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
            this.tilemap.SetTile(position, null);
        }
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }
            row++;
        }
    }

}

