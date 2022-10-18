using Polygons.Shapes;
using Polygons.Visitors;


namespace PolygonDrawerApp.ShapeVisitors
{
    /// <summary>
    /// Ustawia długość krawędzi
    /// </summary>
    public class LengthSetter : PolygonVisitor
    {
        private float length;

        public LengthSetter(float length)
        {
            this.length = length;
        }

        public override void AcceptVisit(Edge edge)
        {
            edge.Length = length;
        }
    }
}
