using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameModes { Normal, Rainbow, Hyper }

public class GameModeSettings
{
    public static GameModes SelectedMode = GameModes.Normal;
}

public class BoardScript : MonoBehaviour
{
    public const int boardWidth = 10;
    public const int boardHeight = 24;
    public const int visibleBoardHeight = 20;
    private const float initialMoveHoldDelay = 0.17f;
    private const float moveHoldDelay = 0.05f;

    public GameObject SquarePrefab;
    public GameObject PauseMenu;
    public GameOverScript gameOverScript;
    public SoundManager soundManager;
    public PiecePreview NextPiecePreview;
    public PiecePreview HoldPiecePreview;
    public TextScript levelText;
    public ScoreScript scoreScript;
    public AudioSource musicPlayer;

    private SquareScript[,] activePieceUI;
    private SquareScript[,] board;
    private GamePiece myPiece;
    private Piece nextPiece;
    private int score;
    private bool gameOver;
    private bool isPaused;
    private bool isScoring;
    private bool canHold;
    private int level = 1;
    private float fallInterval;
    private bool rainbowPieces;

    // Start is called before the first frame update
    void Start()
    {
        switch (GameModeSettings.SelectedMode)
        {
            case GameModes.Rainbow:
                rainbowPieces = true;
                break;
            case GameModes.Hyper:
                SetLevel(15, false);
                break;
        }

        board = CreateSquareGrid(transform, boardWidth, boardHeight, SquarePrefab, true);
        activePieceUI = CreateSquareGrid(transform, boardWidth, boardHeight, SquarePrefab, false);

        myPiece = new GamePiece
        {
            pieceData = GetNextRandomPiece()
        };

        nextPiece = GetNextRandomPiece();
        StartNextPiece();
    }

    // Update is called once per frame
    void Update()
    {
        bool[,] fullBoardSpaces = GetFullBoardSpaces();
        if (!isPaused && !gameOver && !isScoring)
        {
            if (Input.GetButtonDown("RotateLeft"))
            {
                soundManager.PlayRotateSound();
                myPiece.Rotate(true, fullBoardSpaces);
            }
            if (Input.GetButtonDown("RotateRight"))
            {
                soundManager.PlayRotateSound();
                myPiece.Rotate(false, fullBoardSpaces);
            }

            if (Input.GetButtonDown("FastDrop"))
            {
                while (!MovePieceDown()) { }
            }
            if (Input.GetButtonDown("Hold"))
            {
                HoldPiece();
            }

            if (Input.GetButtonDown("Pause"))
            {
                SetPaused(true);
            }

        }

        if (Input.GetButtonDown("MoveLeft"))
        {
            StartCoroutine("MoveLeft");
        }
        if (Input.GetButtonUp("MoveLeft"))
        {
            StopCoroutine("MoveLeft");
        }

        if (Input.GetButtonDown("MoveRight"))
        {
            StartCoroutine("MoveRight");
        }
        if (Input.GetButtonUp("MoveRight"))
        {
            StopCoroutine("MoveRight");
        }

        if (Input.GetButtonDown("SoftDrop"))
        {
            StartCoroutine("SoftDrop");
        }
        if (Input.GetButtonUp("SoftDrop"))
        {
            StopCoroutine("SoftDrop");
        }
        
        ClearActivePieceUI(activePieceUI);
        DrawGamePiece(activePieceUI, myPiece);
    }

    private bool[,] GetFullBoardSpaces()
    {
        bool[,] newGrid = new bool[boardWidth, boardHeight];
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                newGrid[x, y] = board[x, y].GetIsFull();
            }
        }
        return newGrid;
    }

    private Color?[,] GetFullBoardColors()
    {
        Color?[,] newGrid = new Color?[boardWidth, boardHeight];
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                bool full = board[x, y].GetIsFull();
                if (full)
                {
                    newGrid[x, y] = board[x, y].GetColor();
                }
                else
                {
                    newGrid[x, y] = null;
                }
            }
        }
        return newGrid;
    }

    private Piece GetNextRandomPiece(Piece notThis = null)
    {
        return Piece.GetRandomPiece(notThis, rainbowPieces);
    }
    //toUse is set when using Hold piece
    private void StartNextPiece(Piece toUse = null)
    {
        if(toUse == null)
        {
            myPiece.pieceData = nextPiece;
            nextPiece = GetNextRandomPiece(nextPiece);
            NextPiecePreview.SetPiece(nextPiece);
            canHold = true;
        }
        else
        {
            myPiece.pieceData = toUse;
        }
        myPiece.SetAtStart();
        while (myPiece.Overlaps(GetFullBoardSpaces()))
        {
            myPiece.MoveUp();
            if (myPiece.NoSquareOnScreen())
            {
                gameOver = true;
                soundManager.PlayLoseSound();
                gameOverScript.OnGameOver();
                SetHideActivePiece(true);
                musicPlayer.Stop();
                break;
            }
        }
        TryUpdateSpeed();
        ResetFalling();
    }

    private bool MovePieceDown()
    {
        if (isPaused || gameOver || isScoring)
        {
            return false;
        }
        if (IsSolidBelowPiece())
        {
            if (!PlacePiece())
            {
                StartNextPiece();
            }
            return true;
        }
        myPiece.MoveDown();

        ClearActivePieceUI(activePieceUI);
        DrawGamePiece(activePieceUI, myPiece);
        return false;
    }

    public static bool InBounds(int x, int y)
    {
        if (x < 0 || x >= boardWidth)
        {
            return false;
        }
        if(y < 0 || y >= boardHeight)
        {
            return false;
        }
        return true;
    }

    public static SquareScript[,] CreateSquareGrid(Transform parentTransform, int width, int height, GameObject squarePrefab, bool haveBG = false)
    {
        SquareScript[,] newGrid = new SquareScript[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newSquare = Instantiate(squarePrefab, parentTransform);
                float z = haveBG ? 0.1f : 0.0f;
                newSquare.transform.localPosition = new Vector3(x * 1.0f, y * 1.0f, z);
                newGrid[x, y] = newSquare.GetComponent<SquareScript>();
                newGrid[x, y].SetHaveBG(haveBG);
                newGrid[x, y].SetHidden(y > 19);
            }
        }
        return newGrid;
    }

    public static void SetActivePieceUISquare(SquareScript[,] boardToUse, bool val, int x, int y, Color color)
    {
        if((x<0) || (y<0) || (x >= boardToUse.GetLength(0)) || (y >= boardToUse.GetLength(1)))
        {
            return;
        }
        boardToUse[x, y].SetFull(val,color);
    }

    public static void ClearActivePieceUI(SquareScript[,] boardToUse)
    {
        for (int i = 0; i < boardToUse.GetLength(0); i++)
        {
            for (int j = 0; j < boardToUse.GetLength(1); j++)
            {
                boardToUse[i,j].SetFull(false);
            }
        }
    }

    private void SetHideActivePiece(bool val)
    {

        for (int i = 0; i < activePieceUI.GetLength(0); i++)
        {
            for (int j = 0; j < activePieceUI.GetLength(1); j++)
            {
                activePieceUI[i, j].SetHidden(val);
            }
        }
    }

    public static void DrawGamePiece(SquareScript[,] boardToUse, GamePiece piece)
    {
        foreach(Point p in piece.GetBoardSpaceSquares())
        {
            SetActivePieceUISquare(boardToUse, true, p.x, p.y, piece.pieceData.GetColor());
        }
    }

    private bool IsSolidBelowPiece()
    {
        foreach (Point p in myPiece.GetBoardSpaceSquares())
        {
            if (p.y - 1 < 0 || board[p.x, p.y - 1].GetIsFull())
            {
                return true;
            }
        }
        return false;
    }

    //return true if scored
    private bool PlacePiece()
    {
        foreach (Point p in myPiece.GetBoardSpaceSquares())
        {
            board[p.x, p.y].SetFull(true, myPiece.pieceData.GetColor());
        }
        soundManager.PlayPieceDownSound();
        return HandleFullLines();
    }

    public void HoldPiece()
    {
        if (canHold)
        {
            Piece toStore = myPiece.pieceData;
            StartNextPiece(HoldPiecePreview.GetPiece());
            HoldPiecePreview.SetPiece(toStore);
            soundManager.PlayHoldSound();
            canHold = false;
        }
    }

    IEnumerator MoveLeft()
    {
        bool moved;
        if (isPaused || gameOver || isScoring)
        {
            yield return null;
        }
        else
        {
            moved = myPiece.TryMoveLeft(GetFullBoardSpaces());
            if (moved) { soundManager.PlayPieceMoveSound(); }
            yield return new WaitForSeconds(initialMoveHoldDelay);
        }
        while (true)
        {
            if (isPaused || gameOver || isScoring)
            {
                yield return null;
            }
            else
            {
                moved = myPiece.TryMoveLeft(GetFullBoardSpaces());
                if (moved) { soundManager.PlayPieceMoveSound(); }
                yield return new WaitForSeconds(moveHoldDelay);
            }
        }
    }

    IEnumerator MoveRight()
    {
        bool moved;
        if (isPaused || gameOver || isScoring)
        {
            yield return null;
        }
        else
        {
            moved = myPiece.TryMoveRight(GetFullBoardSpaces());
            if (moved) { soundManager.PlayPieceMoveSound(); }
            yield return new WaitForSeconds(initialMoveHoldDelay);
        }
        while (true)
        {
            if (isPaused || gameOver || isScoring)
            {
                yield return null;
            }
            else
            {
                moved = myPiece.TryMoveRight(GetFullBoardSpaces());
                if (moved) { soundManager.PlayPieceMoveSound(); }
                yield return new WaitForSeconds(moveHoldDelay);
            }
        }
    }

    IEnumerator SoftDrop()
    {
        while (true)
        {
            if (isPaused || gameOver || isScoring)
            {
                yield return null;
            }
            else
            {
                MovePieceDown();
                ResetFalling();
                yield return new WaitForSeconds(moveHoldDelay);
            }
        }
    }

    private void TryUpdateSpeed()
    {
        float newInterval = Interval(1.0f + ((level-1) / 14.0f) * 8.5f);

        fallInterval = newInterval;
    }

    private float Interval(float movesPerSecond)
    {
        return 1.0f / movesPerSecond;
    }

    private void ResetFalling()
    {
        int scoreMod = score;

        CancelInvoke("MovePieceDown");
        InvokeRepeating("MovePieceDown", fallInterval, fallInterval);
    }

    private void UpdateLevel()
    {
        int newLevel = level;
        int calculdatedLevel = ((int)(score / 10.0f)) + 1;
        if (calculdatedLevel > 15)
        {
            calculdatedLevel = 15;
        }

        //only increase
        if(calculdatedLevel > level)
        {
            newLevel = calculdatedLevel;
        }

        SetLevel(newLevel,true);
    }
    private void SetLevel(int newLevel, bool playNoise)
    {
        if (newLevel != level)
        {
            level = newLevel;
            if (playNoise)
            {
                soundManager.PlayLevelUpSound();
                levelText.StartFlash();
            }
            levelText.SetText("Level: " + level);
        }
    }

    public void SetPaused(bool val)
    {
        isPaused = val;
        PauseMenu.SetActive(val);
    }

    private bool HandleFullLines()
    {
        List<int> rowsToClear = CheckForLines();
        Color?[,] curBoard = GetFullBoardColors();
        foreach (int rowY in rowsToClear)
        {
            for(int y=rowY; y < boardHeight - 1; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    curBoard[x, y] = curBoard[x, y + 1];
                }
            }
            scoreScript.AddToScore(1);
            score++;
            TryUpdateSpeed();
        }
        if (rowsToClear.Count > 0)
        {
            UpdateLevel();
            SetHideActivePiece(true);
            isScoring = true;
            soundManager.PlayScoreSound();
            FlashRows(rowsToClear);
            CancelInvoke("MovePieceDown");
            StartCoroutine(UpdateBoard(curBoard));
            return true;
        }
        return false;
    }

    IEnumerator UpdateBoard(Color?[,] curBoard)
    {
        yield return new WaitForSeconds(0.5f);
        SetHideActivePiece(false);
        UpdateFullSpaces(curBoard);
        StartNextPiece();
        isScoring = false;
        yield return null;
    }

    private void FlashRows(List<int> rowsToClear)
    {
        foreach (int rowY in rowsToClear)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                board[x, rowY].StartFlash();
            }
        }
    }

    private void UpdateFullSpaces(Color?[,] curBoard)
    {
        for (int j = 0; j < curBoard.GetLength(1); j++)
        {
            for (int i = 0; i < curBoard.GetLength(0); i++)
            {
                board[i, j].SetFull(curBoard[i, j] != null, curBoard[i, j]);
            }
        }
    }

    private List<int> CheckForLines()
    {
        List<int> rowsToClear = new List<int>();
        Color?[,] curBoard = GetFullBoardColors();

        for (int j = curBoard.GetLength(1)-1; j >=0; j--)
        {
            for (int i = 0; i < curBoard.GetLength(0); i++)
            {
                if(curBoard[i, j] == null)
                {
                    break;
                }
                if(i == curBoard.GetLength(0) - 1)
                {
                    rowsToClear.Add(j);
                }
            }
        }
        return rowsToClear;
    }
}
