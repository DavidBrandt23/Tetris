using UnityEngine;
using System.Collections.Generic;

public class Piece
{
    private bool[,] layout;
    private Color color;

    public static Piece GetRandomPiece(Piece notThis = null)
    {
        Piece toReturn = GetPiece(Random.Range(0, 7));
        while (toReturn.color.Equals(notThis?.color))
        {
            toReturn = GetPiece(Random.Range(0, 7));
        }
        return toReturn;
    }

    public Color GetColor()
    {
        return color;
    }

    public static Piece GetPiece(int type)
    {
        if (type < 0 || type > 6)
        {
            throw new System.Exception("invalid type passed to GetPiece");
        }
        List<Piece> pieces = CreatePieceList();

        return pieces[type];
    }

    private static List<Piece> CreatePieceList()
    {
        List<Piece> pieces = new List<Piece>();
        Piece t = new Piece
        {
            layout = new bool[,] {
            {false,true,false},
            {true,true,true},
            {false,false,false}
            },
            color = Color.magenta
        };
        pieces.Add(t);

        Piece i = new Piece
        {
            layout = new bool[,] {
            {false,false,false,false},
            {true,true,true,true},
            {false,false,false,false},
            {false,false,false,false}
            },
            color = Color.cyan
        };
        pieces.Add(i);

        Piece s1 = new Piece
        {
            layout = new bool[,] {
            {false,true,true},
            {true,true,false},
            {false,false,false}
            },
            color = Color.green
        };
        pieces.Add(s1);

        Piece s2 = new Piece
        {
            layout = new bool[,] {
            {true,true,false},
            {false,true,true},
            {false,false,false}
            },
            color = Color.red
        };
        pieces.Add(s2);

        Piece square = new Piece
        {
            layout = new bool[,] {
            {true,true },
            {true,true},
            },
            color = Color.yellow
        };
        pieces.Add(square);

        Piece l1 = new Piece
        {
            layout = new bool[,] {
            {true,false,false},
            {true,true,true},
            {false,false,false}
            },
            color = Color.blue
        };
        pieces.Add(l1);

        Piece l2 = new Piece
        {
            layout = new bool[,] {
            {false,false,true},
            {true,true,true},
            {false,false,false}
            },
            color = new Color(255.0f / 256.0f, 128.0f / 256.0f, 0.0f, 1.0f) //orange
        };
        pieces.Add(l2);
        return pieces;
    }

    public bool[,] GetLayout(int rotations = 0)
    {
        bool[,] rotLayout = layout;
        for(int i = 0; i < rotations; i++)
        {
            rotLayout = RotateArrayCntClockwise(rotLayout);
        }
        return rotLayout;
    }

    private static bool[,] RotateArrayCntClockwise(bool[,] src)
    {
        int width;
        int height;
        bool[,] dst;

        width = src.GetUpperBound(0) + 1;
        height = src.GetUpperBound(1) + 1;
        dst = new bool[height, width];

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                int newRow;
                int newCol;

                newRow = col;
                newCol = height - (row + 1);

                dst[newCol, newRow] = src[col, row];
            }
        }

        return dst;
    }
}
