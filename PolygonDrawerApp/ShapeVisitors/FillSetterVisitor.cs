using Polygons.Shapes;
using Polygons.Visitors;

namespace PolygonDrawerApp.ShapeVisitors
{
    public class FillSetter : PolygonVisitor
    {
        public static FillSetter Instance { get; } = new FillSetter();

        private FillSetter()
        {
        }

        public override void AcceptVisit(Polygon polygon)
        {
            polygon.IsFilled = !polygon.IsFilled;
        }
    }
}
