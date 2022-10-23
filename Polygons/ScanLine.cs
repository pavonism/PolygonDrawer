using Polygons.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Polygons
{
    public class ScanLine
    {
        private static void SortEdges(List<Edge> edges, Edge[] scanLines, int minY)
        {
            foreach (var edge in edges)
            {
                
            }
        }

        public static void FillPolygon(Polygon polygon, List<Edge> edges)
        {
            polygon.GetExtremePoints(out var minPoint, out var maxPoint);
            Edge[] scanLines = new Edge[(int)(maxPoint.Y - maxPoint.Y)];


            SortEdges(edges, scanLines, (int)minPoint.Y);
        }
    }
}
