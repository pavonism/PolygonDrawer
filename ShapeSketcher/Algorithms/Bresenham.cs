
namespace ShapeSketcher.Algorithms
{
    public static class Bresenham
    {
        public static void DrawLine(int x1, int y1, int x2, int y2, Bitmap bitmap, Color color)
        {
            // zmienne pomocnicze
            int d, dx, dy, dxDiag, dxStraight, xi, yi;
            int x = x1, y = y1;
            // ustalenie kierunku rysowania
            if (x1 < x2)
            {
                xi = 1;
                dx = x2 - x1;
            }
            else
            {
                xi = -1;
                dx = x1 - x2;
            }
            // ustalenie kierunku rysowania
            if (y1 < y2)
            {
                yi = 1;
                dy = y2 - y1;
            }
            else
            {
                yi = -1;
                dy = y1 - y2;
            }
            // pierwszy piksel
            bitmap.SetPixel(x, y, color);
            // oś wiodąca OX
            if (dx > dy)
            {
                dxDiag = (dy - dx) * 2;
                dxStraight = dy * 2;
                d = dxStraight - dx;
                // pętla po kolejnych x
                while (x != x2)
                {
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += dxDiag;
                    }
                    else
                    {
                        d += dxStraight;
                        x += xi;
                    }
                    bitmap.SetPixel(x, y, color);
                }
            }
            // oś wiodąca OY
            else
            {
                dxDiag = (dx - dy) * 2;
                dxStraight = dx * 2;
                d = dxStraight - dy;
                // pętla po kolejnych y
                while (y != y2)
                {
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += dxDiag;
                    }
                    else
                    {
                        d += dxStraight;
                        y += yi;
                    }
                    bitmap.SetPixel(x, y, color);
                }
            }
        }

        public static void DrawLineModified(int x1, int y1, int x2, int y2, Bitmap bitmap, Color color)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;

            if (dx < 0) dx1 = dx2 = -1; // II i III ćwiartka (x2 na lewo od x1)
            else if (dx > 0) dx1 = dx2 = 1; // I i IV ćwiartka (x2 na prawo od x1)

            if (dy < 0) dy1 = -1; // III i IV ćwiartka (y2 poniżej y1)
            else if (dy > 0) dy1 = 1; // I i II ćwiartka (y2 powyżej y1)

            // sprawdzamy po której współrzędnej jest dalej 
            // chcemy iśc po tej, która jest dalej, żeby więcej punktów móc ponastawiać
            // + to załatwia przypadek, w którym mamy pionową linię
            int longest = Math.Abs(dx);
            int shortest = Math.Abs(dy);
            if (!(longest > shortest))
            {
                longest = Math.Abs(dy);
                shortest = Math.Abs(dx);
                if (dy < 0) dy2 = -1;
                else if (dy > 0) dy2 = 1;
                dx2 = 0;
            }

            int d = longest >> 1; // Mnożymy razy 2
            for (int i = 0; i <= longest; i++)
            {
                bitmap.SetPixel(x1, y1, color);
                d += shortest;
                // idziemy po krótszej różnicy i po dłuższej - na ukos
                // za każdym krokiem po dłuższej współrzędnej dodajemy krok po krótszej współrzędnej, 
                // jeżeli suma przekroczy longest, to znaczy, że musimy pójść po krótszej współrzędnej 
                if (!(d < longest))
                {
                    d -= longest;
                    x1 += dx1;
                    y1 += dy1;
                }
                // idziemy po prostej - tylko po jednej współrzędnej 
                else
                {
                    x1 += dx2;
                    y1 += dy2;
                }
            }
        }
    }
}