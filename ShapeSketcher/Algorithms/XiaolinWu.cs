using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeSketcher.Algorithms
{
    /// <summary>
    /// Umożliwia rysowanie linii z antyaliasingiem z użyciem algorytmu XiaolinWu
    /// </summary>
    public static class XiaolinWu
    {
        private static float Frac(float x)
        {
            return x > 0 ? x - (int)x : x - ((int)x + 1);
        }

        private static float RFrac(float x)
        {
            return 1 - Frac(x);
        }

        private static Color GetColorWithBrightness(float percentage)
        {
            return Color.FromArgb(255, (int)(percentage * 255), (int)(percentage * 255), (int)(percentage * 255));
        }

        public static void DrawLine(int x1, int y1, int x2, int y2, Bitmap bitmap, Color color)
        {
            bool isDYlonger = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);

            if (isDYlonger)
            {
                (x1, y1) = (y1, x1);
                (x2, y2) = (y2, x2);
            }
            if (x1 > x2)
            {
                (x1, x2) = (x2, x1);
                (y1, y2) = (y2, y1);
            }

            float dx = x2 - x1;
            float dy = y2 - y1;
            float m = dx == 0.0 ? 1 : dy / dx;
            float y = y1;

            if (isDYlonger)
            {
                for (int x = x1; x <= x2; x++)
                {
                    bitmap.SetPixel((int)y + 1, x, GetColorWithBrightness(RFrac(y)));
                    bitmap.SetPixel((int)y, x, GetColorWithBrightness(Frac(y)));

                    y += m;
                }
            }
            else
            {
                for (int x = x1; x <= x2; x++)
                {
                    bitmap.SetPixel(x, (int)y + 1, GetColorWithBrightness(RFrac(y)));
                    bitmap.SetPixel(x, (int)y, GetColorWithBrightness(Frac(y)));
                    y += m;
                }
            }
        }
    }
}
