
namespace ShapeSketcher.Algorithms
{
    /// <summary>
    /// Klasa udostępniająca postawowe operacje geometryczne
    /// </summary>
    public static class Geometrics
    {
        /// <summary>
        /// Sprawdza, czy punkt <paramref name="third"/> jest na prawo czy na lewo 
        /// od odcinka z punktu <paramref name="first"/> do punktu <paramref name="second"/>
        /// </summary>
        /// <returns>
        /// Jeżeli punkt <paramref name="third"/> jest na lewo to -1, na prawo 1, współliniowy 0
        /// </returns>
        public static int Turn(PointF first, PointF second, PointF third)
        {
            double value = (second.X - first.X) * (third.Y - first.Y) - (second.Y - first.Y) * (third.X - first.X);
            return Math.Abs(value) < 1e-10 ? 0 : value < 0 ? -1 : 1;
        }

        /// <summary>
        /// Oblicza iloczyn wektorowy dwóch wektorów osadzonych w punkcie <paramref name="o"/>
        /// </summary>
        public static float Cross(PointF o, PointF a, PointF b)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
        }

        /// <summary>
        /// Oblicza iloczyn wektorowy dwóch wektorów osadzonych w punkcie <paramref name="o"/>
        /// </summary>
        public static float Dot(PointF a, PointF b)
        {
            return a.X * b.X  + a.Y * b.Y;
        }

        /// <summary>
        /// Oblicza długość linii z <paramref name="from"/> do <paramref name="to"/>.
        /// </summary>
        public static float LineLength(PointF from, PointF to)
        {
            return (float)Math.Sqrt(Math.Pow(to.X - from.X, 2) + Math.Pow(to.Y - from.Y, 2));
        }

        public static PointF Minus(this PointF thisPoint, PointF point)
        {
            return new PointF(thisPoint.X - point.X, thisPoint.Y - point.Y);
        }
    }
}
