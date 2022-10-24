using Polygons.Shapes;

namespace Polygons.Visitors
{
    public abstract class PolygonVisitor : IPolygonVisitor
    {
        public virtual void AcceptVisit(Edge edge)
        {
        }

        public virtual void AcceptVisit(Polygon polygon)
        {
        }

        public virtual void AcceptVisit(Vertex vertex)
        {
        }
    }
}
