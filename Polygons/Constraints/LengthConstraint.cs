using Polygons.Shapes;
using System.Drawing;

namespace Polygons.Constraints
{

    public class LengthConstraint : IConstraint
    {
        public static LengthConstraint Instance = new LengthConstraint();
        public string Symbol => ConstraintSymbols.ConstLength;

        public object Id => string.Empty;

        private LengthConstraint()
        {
        }

        public void Check(Vertex movedVertex, Edge movedEdge)
        {
            if (!movedEdge.FixedLength.HasValue)
                return;

            movedEdge.Lock = true;
            AdjustEdgeLength(movedEdge, movedEdge.FixedLength.Value, movedVertex);
            movedEdge.Lock = false;
        }

        public static void AdjustEdgeLength(Edge edge, float fixedLength)
        {
            AdjustEdgeLength(edge, fixedLength, edge.To);
        }

        public static void AdjustEdgeLength(Edge edge, float fixedLength, Vertex fixedVertex)
        {
            if (edge.Length == fixedLength)
                return;

            var secondVertex = edge.From != fixedVertex ? edge.From : edge.To;

            float scale = fixedLength / edge.Length;

            PointF newLocation = new PointF()
            {
                X = (secondVertex.X - fixedVertex.X) * scale + fixedVertex.X,
                Y = (secondVertex.Y - fixedVertex.Y) * scale + fixedVertex.Y
            };

            secondVertex.MoveTo(newLocation);
        }

        public void Delete(Edge request)
        {
        }
    }

}
