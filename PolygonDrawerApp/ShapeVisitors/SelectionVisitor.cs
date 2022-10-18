using PolygonDrawerApp.Components;
using Polygons.Shapes;
using Polygons.Visitors;

namespace PolygonDrawerApp.ShapeVisitors
{
    /// <summary>
    /// Zmienia dostępne narzędzia w zależności od wybranego kształtu
    /// </summary>
    public class SelectionVisitor : IPolygonVisitor
    {
        private readonly LengthOperator lengthOperator;

        public SelectionVisitor(LengthOperator lengthOperator)
        {
            this.lengthOperator = lengthOperator;
        }

        public void AcceptVisit(Edge edge)
        {
            lengthOperator.Show();
            lengthOperator.Lock = edge.FixedLength.HasValue;
            lengthOperator.Value = (decimal)edge.Length;
        }

        public void AcceptVisit(Polygon polygon)
        {
            lengthOperator.Hide();
        }

        public void AcceptVisit(Vertex vertex)
        {
            lengthOperator.Hide();
        }
    }
}
