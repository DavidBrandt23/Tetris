using UnityEngine;
using System.Collections.Generic;

public class Piece
{
    private bool[,] layout;
    private Color color;

    public static Piece GetRandomPiece(Piece notThis = null, bool withRandomColor = false, bool useAltShapes= false)
    {
        List<Piece> pieces = CreatePieceList(useAltShapes);
        Piece toReturn = pieces[Random.Range(0, pieces.Count)];
        while (toReturn.IsSameShape(notThis))
        {
            toReturn = pieces[Random.Range(0, pieces.Count)];
        }

        if (withRandomColor)
        {
            toReturn.color = RandomColor();
        }
        return toReturn;
    }
    private static Color RandomColor()
    {
        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);
        return new Color(r, g, b);
    }

    public Color GetColor()
    {
        return color;
    }

    private bool IsSameShape(Piece other)
    {
        if(other == null)
        {
            return false;
        }
        bool[,] otherLayout = other.layout;
        if (layout.GetLength(0) != otherLayout.GetLength(0) ||
            layout.GetLength(1) != otherLayout.GetLength(1))
        {
            return false;
        }
        for (int x = 0; x < layout.GetLength(0); x++)
        {
            for(int y = 0; y < layout.GetLength(1); y++)
            {
                if(layout[x,y] != otherLayout[x, y])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static List<Piece> CreatePieceList(bool altList = false)
    {
        List<Piece> pieces = new List<Piece>();
        if (!altList)
        {
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
        }
        else
        {
            Piece t = new Piece
            {
                layout = new bool[,] {
            {true,false,true},
            {true,true,true},
            {false,false,false}
            },
                color = Color.magenta
            };
            pieces.Add(t);

            Piece i = new Piece
            {
                layout = new bool[,] {
            {false,false,false,true},
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
            {false,true,false},
            {true,true,false},
            {false,false,false}
            },
                color = Color.green
            };
            pieces.Add(s1);

            Piece s2 = new Piece
            {
                layout = new bool[,] {
            {true,true,true},
            {false,true,true},
            {false,false,false}
            },
                color = Color.red
            };
            pieces.Add(s2);

            Piece square = new Piece
            {
                layout = new bool[,] {
            {true,false },
            {false,true},
            },
                color = Color.yellow
            };
            pieces.Add(square);

            Piece l1 = new Piece
            {
                layout = new bool[,] {
            {true,false,false},
            {true,true,true},
            {false,false,true}
            },
                color = Color.blue
            };
            pieces.Add(l1);

            Piece l2 = new Piece
            {
                layout = new bool[,] {
            {false,true,true},
            {true,true,true},
            {false,false,false}
            },
                color = new Color(255.0f / 256.0f, 128.0f / 256.0f, 0.0f, 1.0f) //orange
            };
            pieces.Add(l2);
        }
      
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
