using Polygons.Shapes;

namespace Polygons.Visitors
{
    public interface IPolygonVisitor
    {
        void AcceptVisit(Edge edge);
        void AcceptVisit(Polygon polygon);
        void AcceptVisit(Vertex vertex);
    }
}