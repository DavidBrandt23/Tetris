using UnityEngine;

public class PiecePreview : MonoBehaviour
{
    public GameObject SquarePrefab;
    public Transform boardPos;

    private SquareScript[,] previewBoard;
    private Piece storedPiece;

    void Start()
    {
        InitBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Piece GetPiece()
    {
        return storedPiece;
    }

    public void SetPiece(Piece piece)
    {
        InitBoard();
        storedPiece = piece;

        GamePiece myPiece = new GamePiece
        {
            pieceData = storedPiece
        };
        myPiece.position = new Point(0, 3);
        int pieceSize = myPiece.pieceData.GetLayout().GetLength(0);
        if (pieceSize == 2)
        {
            boardPos.transform.localPosition = new Vector3(-0.5f, 0.0f, 0.0f);
        }
        if (pieceSize == 3)
        {
            boardPos.transform.localPosition = new Vector3(-1.0f, 0.0f, 0.0f);
        }
        if (pieceSize == 4)
        {
            boardPos.transform.localPosition = new Vector3(-1.5f, 0.5f, 0.0f);
        }
        BoardScript.ClearActivePieceUI(previewBoard);
        BoardScript.DrawGamePiece(previewBoard, myPiece);
    }

    private void InitBoard()
    {
        if(previewBoard == null)
        {
            previewBoard = BoardScript.CreateSquareGrid(boardPos.transform, 4, 4, SquarePrefab, false);

        }
    }
}
