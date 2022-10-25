using PolygonDrawer.Components;
using Polygons.Shapes;
using Polygons.Visitors;

namespace PolygonDrawer.ShapeVisitors
{
    /// <summary>
    /// Zmienia dostępne narzędzia w zależności od wybranego kształtu
    /// </summary>
    public class SelectionVisitor : IPolygonVisitor
    {
        private readonly LengthOperator lengthOperator;
        private readonly Button bezierButton;

        public SelectionVisitor(LengthOperator lengthOperator, Button bezierButton)
        {
            this.lengthOperator = lengthOperator;
            this.bezierButton = bezierButton;
        }

        public void AcceptVisit(Edge edge)
        {
            lengthOperator.Show();
            lengthOperator.Lock = edge.FixedLength.HasValue;
            lengthOperator.Value = (decimal)edge.Length;
            if(!edge.IsBezier)
                bezierButton.Visible = true;
        }

        public void AcceptVisit(Polygon polygon)
        {
            Reset();
        }

        public void AcceptVisit(Vertex vertex)
        {
            Reset();
        }

        public void Reset()
        {
            lengthOperator.Hide();
            bezierButton.Visible = false;
        }
    }
}
