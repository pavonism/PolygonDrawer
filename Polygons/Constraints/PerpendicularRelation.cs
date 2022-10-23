using Polygons.Shapes;
using ShapeSketcher.Algorithms;
using System.Drawing;

namespace Polygons.Constraints
{
    public class PerpendicularRelation : EdgeRelation
    {
        public override string Symbol => ConstraintSymbols.Perpendicular;

        private const float angle = (float)Math.PI / 2;

        public PerpendicularRelation(Edge brother, Edge sister, object id) : base(brother, sister, id)
        {
            Check(sister.To, sister);
        }

        public override void Check(Vertex movedVertex, Edge fixedEdge)
        {

            Vertex secondVertex = fixedEdge.GetSecondVertex(movedVertex);
            Edge secondEdge = brother != fixedEdge ? brother : sister;
            float rotation = angle;

            float distFrom = Geometrics.Cross(secondVertex.Location, secondEdge.From.Location, movedVertex.Location) / fixedEdge.Length;
            float distTo = Geometrics.Cross(secondVertex.Location, secondEdge.To.Location, movedVertex.Location) / fixedEdge.Length;
            Vertex vertexToMove = Math.Abs(distFrom) > Math.Abs(distTo) ? secondEdge.From : secondEdge.To;
            Vertex fixedVertex = secondEdge.To != vertexToMove ? secondEdge.To : secondEdge.From;

            if (Math.Abs(Geometrics.Dot(movedVertex.Location.Minus(secondVertex.Location), vertexToMove.Location.Minus(fixedVertex.Location))) < 1)
                return;

            if (Geometrics.Turn(secondVertex.Location, movedVertex.Location, vertexToMove.Location) < 0)
                rotation = -rotation;

            fixedEdge.ConstraintLock = true;

            // Translate to origin
            var x = movedVertex.X - secondVertex.X;
            var y = movedVertex.Y - secondVertex.Y;
            // Rotate 
            var rotatedX = (float)(Math.Cos(rotation) * x - Math.Sin(rotation) * y);
            var rotatedY = (float)(Math.Sin(rotation) * x + Math.Cos(rotation) * y);
            // Scale
            rotatedX *= secondEdge.Length / fixedEdge.Length;
            rotatedY *= secondEdge.Length / fixedEdge.Length;

            var newPoint = new PointF()
            {
                X = rotatedX + fixedVertex.X,
                Y = rotatedY + fixedVertex.Y,
            };

            vertexToMove.MoveTo(newPoint);

            fixedEdge.ConstraintLock = false;
        }
    }
}
