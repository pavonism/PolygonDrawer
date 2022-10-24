using Polygons.Shapes;
using Polygons.Visitors;

namespace PolygonDrawer.ShapeVisitors
{
    public class BezierSetter : PolygonVisitor
    {
        public override void AcceptVisit(Edge edge)
        {
            edge.IsBezier = !edge.IsBezier;
        }
    }
}
