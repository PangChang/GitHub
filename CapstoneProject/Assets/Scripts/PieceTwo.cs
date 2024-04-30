using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PieceTwo : MonoBehaviour
{
    public GameBoardTwo boardTwo { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int position { get; private set; }//Tilemaps uses Vector3Ints instead of Vector2Ints
    public Vector3Int[] cells { get; private set; } //array of cells
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float moveTime;
    private float lockTime;

    public void Initialize(GameBoardTwo boardTwo, Vector3Int position, TetrominoData data)
    {
        this.boardTwo = boardTwo;
        this.position = position;
        this.data = data;


        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.moveTime = Time.time + this.moveDelay;
        this.lockTime = 0f;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i]; //(Vector3Int) cast -> needs to be vector3Int to be able to set on Tilemaps
        }

    }

    private void Update()
    {
        if (!pauseMenu.isPaused)
        {
            this.boardTwo.Clear(this); //clears piece from the board
            this.lockTime += Time.deltaTime;

            //Player Two Inputs
            if (Input.GetKeyDown(KeyCode.U))
            {
                Rotate(-1);
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                Rotate(1);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                Move(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Move(Vector2Int.right);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                HardDrop();
            }

            if (Time.time > this.moveTime)
            {
                HandleMoveInputs();
            }

            if (Time.time >= this.stepTime)
            {
                Step();
            }

            this.boardTwo.Set(this); //sets piece in the board
        }
    }


    private void HandleMoveInputs()
    {
        if (Input.GetKey(KeyCode.K))
        {
            if (Move(Vector2Int.down))
            {

                stepTime = Time.time + stepDelay;
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////////////
    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);

        if (this.lockTime > this.lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        this.boardTwo.Set(this);
        this.boardTwo.ClearLines();
        this.boardTwo.SpawnPiece();
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }

        Lock();
    }
    //////////////////////////////////////////////////////////////////////////////////

    //needs to be bool to make sure functions can actually happen
    private bool Move(Vector2Int translation)//make sure movement is valid, Gameboard will allow you to measure correctly
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = boardTwo.IsValidPosition(this, newPosition);
        if (valid)
        {
            this.position = newPosition;
            lockTime = 0f;

        }
        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex; //to keep direction if rotation does not work
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction)) //will go back into origional rotation
        {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }

    }

    private void ApplyRotationMatrix(int direction)
    {

        for (int i = 0; i < this.cells.Length; i++) //using this.cells, not this.data.cells
        {
            Vector3 cell = this.cells[i];
            int x, y;

            switch (this.data.tetromino) //data in array, multiply by itself
            {
                //ceilToInt to round upward
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;

                default:    //round for rotation matrix
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }
        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0)); //allows wallkicks based on recent information, no need for hard code
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }

}
