using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PieceTou : MonoBehaviour
{
    public GameBoardTou gameBoardTou { get; private set; }
    public GameObject ghostObj;
    public TetrominoDataTou dataTou { get; private set; }
    public Vector3Int positionTou { get; private set; }//Tilemaps uses Vector3Ints instead of Vector2Ints
    public Vector3Int[] cellsTou { get; private set; } //array of cells
    public int rotationIndexTou { get; private set; }
    public AudioSource sourceOne;
    public AudioClip soundPlayed;

    public bool ifPlayedSound = false;

    public float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float moveTime;
    private float lockTime;

    public void Initialize(GameBoardTou gameBoardTou, Vector3Int positionTou, TetrominoDataTou dataTou)
    {
        this.gameBoardTou = gameBoardTou;
        this.positionTou = positionTou;
        this.dataTou = dataTou;


        this.rotationIndexTou = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.moveTime = Time.time + this.moveDelay;
        this.lockTime = 0f;

        if (this.cellsTou == null)
        {
            this.cellsTou = new Vector3Int[dataTou.cellsTou.Length];
        }

        for (int i = 0; i < dataTou.cellsTou.Length; i++)
        {
            this.cellsTou[i] = (Vector3Int)dataTou.cellsTou[i]; //(Vector3Int) cast -> needs to be vector3Int to be able to set on Tilemaps
        }

    }

    private void Update()
    {
        if (!pauseMenu.isPaused) //only actively able to use inputs, if the game is not paused (!pauseMenu.isPaused)
        {
            this.gameBoardTou.Clear(this); //clears piece from the board
            this.lockTime += Time.deltaTime;

            //Player Inputs
            if (Input.GetKeyDown(KeyCode.W))
            {
                Rotate(-1);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Rotate(1);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(Vector2Int.right);
            }

            if (Input.GetKeyDown(KeyCode.Space))
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

            this.gameBoardTou.Set(this); //sets piece in the board

            if (Score.score >= 5 && Score.score <= 7)//first level, will be here
            {
                if (!ifPlayedSound)
                {
                    
                    sourceOne.PlayOneShot(soundPlayed);
                    ifPlayedSound = true;
                }
                ghostObj.SetActive(false);//to turn off ghost piece
            }

            if (Score.score >= 8 && Score.score <= 9)//to allow the level up notification to go off again.
            {
                ifPlayedSound = false;
            }
            else if (Score.score >= 10 && Score.score <= 13)
            {
                if(!ifPlayedSound)
                {
                    sourceOne.PlayOneShot(soundPlayed);
                    ifPlayedSound = true;
                }
                stepDelay = 0.75f;
            }
            
            if (Score.score >= 14 && Score.score <= 19) //to allow the level up notification to go off again.
            {
                ifPlayedSound = false;
            }
            else if (Score.score >= 20 && Score.score <= 23)
            {
                 if (!ifPlayedSound)
                 {
                    sourceOne.PlayOneShot(soundPlayed);
                    ifPlayedSound = true;
                 }
                 stepDelay = 0.5f;
            }

            if (Score.score >= 24 && Score.score <= 29) //to allow the level up notification to go off again.
            {
                ifPlayedSound = false;
            }
            else if (Score.score >= 30 && Score.score <= 33)
            {
                 if (!ifPlayedSound)
                 {
                    sourceOne.PlayOneShot(soundPlayed);
                    ifPlayedSound = true;
                 }
                stepDelay = 0.25f;
            } 

            if (Score.score >= 34 && Score.score <= 39) //to allow the level up notification to go off again.
            {
                 ifPlayedSound = false;
            }
            else if (Score.score >= 40 && Score.score <= 43)
            {
                 if (!ifPlayedSound)
                 {
                     sourceOne.PlayOneShot(soundPlayed);
                     ifPlayedSound = true;
                 }
                 stepDelay = 0.075f;
            }

            if (Score.score >= 44 && Score.score <= 49) //to allow the level up notification to go off again.
            {
                 ifPlayedSound = false;
            }
            else if (Score.score >= 50)
            {
                 if (!ifPlayedSound)
                 {
                     sourceOne.PlayOneShot(soundPlayed);
                     ifPlayedSound = true;
                 }
                 stepDelay = 0.055f;
            }

        }
    }


    private void HandleMoveInputs()
    {
        if (Input.GetKey(KeyCode.S)) //still allows movement (not like harddrop Space)
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
        this.gameBoardTou.Set(this);
        this.gameBoardTou.ClearLines();
        this.gameBoardTou.SpawnPiece();
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
        Vector3Int newPosition = this.positionTou;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = gameBoardTou.IsValidPosition(this, newPosition);
        if (valid)
        {
            this.positionTou = newPosition;
            lockTime = 0f;

        }
        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndexTou; //to keep direction if rotation does not work
        this.rotationIndexTou = Wrap(this.rotationIndexTou + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndexTou, direction)) //will go back into origional rotation
        {
            this.rotationIndexTou = originalRotation;
            ApplyRotationMatrix(-direction);
        }

    }

    private void ApplyRotationMatrix(int direction)
    {

        for (int i = 0; i < this.cellsTou.Length; i++) //using this.cells, not this.data.cells
        {
            Vector3 cell = this.cellsTou[i];
            int x, y;

            switch (this.dataTou.tetrominoTou) //data in array, multiply by itself
            {
                //ceilToInt to round upward
                case TetrominoTou.I:
                case TetrominoTou.O:
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

            this.cellsTou[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndexTou, int rotationDirectionTou)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndexTou, rotationDirectionTou);

        for (int i = 0; i < this.dataTou.wallKicksTou.GetLength(1); i++)
        {
            Vector2Int translation = this.dataTou.wallKicksTou[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndexTou, int rotationDirectionTou)
    {
        int wallKickIndex = rotationIndexTou * 2;

        if (rotationDirectionTou < 0)
        {
            wallKickIndex--;
        }
        return Wrap(wallKickIndex, 0, this.dataTou.wallKicksTou.GetLength(0)); //allows wallkicks based on recent information
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
