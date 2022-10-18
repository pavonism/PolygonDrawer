using PolygonDrawer;
using Polygons.Constraints;
using Polygons.Shapes;
using Polygons.Visitors;

namespace PolygonDrawerApp.ShapeVisitors
{
    public class ConstLengthSetter : PolygonVisitor
    {
        private readonly float? fixedLength;

        public ConstLengthSetter(float? fixedLength)
        {
            this.fixedLength = fixedLength;
        }
        public override void AcceptVisit(Edge edge)
        {
            edge.FixedLength = fixedLength;
            if (fixedLength == null)
                edge.RemoveConstraint(LengthConstraint.Instance);
            else
                edge.AddConstraint(LengthConstraint.Instance);
        }
    }
}
