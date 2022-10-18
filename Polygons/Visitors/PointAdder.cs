using Polygons.Shapes;
using System.Drawing;

namespace Polygons.Visitors
{
    public class PointAdder : PolygonVisitor
    {
        PointF newPoint;

        public PointAdder(PointF newPoint)
        {
            this.newPoint = newPoint;
        }

        public override void AcceptVisit(Edge edge)
        {
            edge.AddNewVertex(newPoint);
        }
    }
}
