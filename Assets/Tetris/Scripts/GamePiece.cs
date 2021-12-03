using System.Collections.Generic;

public class GamePiece
{
    public Piece pieceData;
    public Point position;
    public int curRotation;

    public GamePiece()
    {
        position = new Point(0, 0);
    }

    public void SetAtStart()
    {
        if (pieceData.GetLayout().GetLength(0) > 2)
        {
            position = new Point(3, BoardScript.visibleBoardHeight - 1);
        }
        else
        {
            position = new Point(4, BoardScript.visibleBoardHeight - 1);
        }
        curRotation = 0;
    }

    public List<Point> GetBoardSpaceSquares()
    {
        List<Point> locs = new List<Point>();
        bool[,] layout = pieceData.GetLayout(curRotation);
        for (int i = 0; i < layout.GetLength(0); i++)
        {
            for (int j = 0; j < layout.GetLength(1); j++)
            {
                bool pieceHasBlock = layout[i, j];
                if (pieceHasBlock)
                {
                    int relX = j + position.x;
                    int relY = -1 * i + position.y;
                    locs.Add(new Point(relX, relY));
                }
            }
        }
        return locs;
    }

    public void MoveUp()
    {
        position = new Point(position.x, position.y + 1);
    }

    public void MoveDown()
    {
        position = new Point(position.x, position.y - 1);
    }

    public bool Overlaps(bool[,] fullBoardSpaces)
    {
        foreach (Point p in GetBoardSpaceSquares())
        {
            if (!OkaySpaceToMove(p.x, p.y, fullBoardSpaces))
            {
                return true;
            }
        }
        return false;
    }

    private bool OkaySpaceToMove(int x, int y, bool[,] fullBoardSpaces)
    {
        if (!BoardScript.InBounds(x, y) || fullBoardSpaces[x, y])
        {
            return false;
        }
        return true;
    }

    public bool TryMoveLeft(bool[,] fullBoardSpaces)
    {
        position.x--;
        if (Overlaps(fullBoardSpaces))
        {
            position.x++;
            return false;
        }
        return true;
    }

    public bool TryMoveRight(bool[,] fullBoardSpaces)
    {
        position.x++;
        if (Overlaps(fullBoardSpaces))
        {
            position.x--;
            return false;
        }
        return true;
    }

    public void Rotate(bool CCW, bool[,] fullBoardSpaces)
    {
        if (CCW)
        {
            curRotation++;
        }
        else
        {
            curRotation--;
        }
        if (curRotation < 0)
        {
            curRotation = 3;
        }
        curRotation = curRotation % 4;
        if (Overlaps(fullBoardSpaces))
        {
            Rotate(!CCW, fullBoardSpaces);
        }
    }

    public bool NoSquareOnScreen()
    {
        foreach (Point p in GetBoardSpaceSquares())
        {
            if (p.y < BoardScript.visibleBoardHeight)
            {
                return false;
            }
        }
        return true;
    }

    public bool OffTop()
    {
        foreach (Point p in GetBoardSpaceSquares())
        {
            if (p.y >= BoardScript.visibleBoardHeight)
            {
                return true;
            }
        }
        return false;
    }
}