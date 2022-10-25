
namespace ShapeSketcher.Algorithms
{
    public class Bezier
    {
        private const float DrawingStep = 0.001f;

        public static void Draw(PointF[] points, Bitmap bitmap, Color color)
        {
            if (points.Length != 4)
                return;

            PointF A0 = points[0];
            PointF A1 = new PointF(3 * (points[1].X - points[0].X), 3 * (points[1].Y - points[0].Y));
            PointF A2 = new PointF(3 * (points[2].X - 2 * points[1].X + points[0].X), 3 * (points[2].Y - 2 * points[1].Y + points[0].Y));
            PointF A3 = new PointF(points[3].X - 3 * points[2].X + 3 * points[1].X - points[0].X, points[3].Y - 3 * points[2].Y + 3 * points[1].Y - points[0].Y);

            for (float t = 0; t <= 1; t += DrawingStep)
            {
                int x = (int)(A0.X + t * (A1.X + t * (A2.X + (A3.X * t))));
                int y = (int)(A0.Y + t * (A1.Y + t * (A2.Y + (A3.Y * t))));

                if (0 < x && x < bitmap.Width && 0 < y && y < bitmap.Height)
                    bitmap.SetPixel(x, y, color);
            }
        }
    }
}
