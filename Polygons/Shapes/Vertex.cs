using Polygons.Visitors;
using ShapeSketcher;
using System.Drawing;

namespace Polygons.Shapes
{

    public class Vertex : IPolygonShape
    {
        private bool isSelected;
        private bool locked;
        public float X { get; set; }
        public float Y { get; set; }
        public PointF Location => new(X, Y);

        private readonly List<Edge> edges = new();

        public event Action<IPolygonShape> OnShapeDelete;
        public event OnVertexDeleteHandler? OnVertexDelete;
        public event Action<Vertex> OnVertexMove;

        public Vertex(PointF point)
        {
            X = point.X;
            Y = point.Y;
        }

        public void Render(Bitmap drawingContext, RenderMode renderMode)
        {
            RectangleF rectangle = new RectangleF(X - ShapesConstants.VertexPointRadius, Y - ShapesConstants.VertexPointRadius,
                2 * ShapesConstants.VertexPointRadius, 2 * ShapesConstants.VertexPointRadius);

            using (var g = Graphics.FromImage(drawingContext))
            {
                if (isSelected)
                    g.FillEllipse(ShapesConstants.SelectionBrush, rectangle);
                else
                    g.FillEllipse(ShapesConstants.DefaultBrush, rectangle);

                g.DrawEllipse(ShapesConstants.DefaultPen, rectangle);
            }
        }

        public bool HitTest(PointF point)
        {
            return Math.Pow(Math.Abs(X - point.X), 2) + Math.Pow(Math.Abs(Y - point.Y), 2) <= Math.Pow(ShapesConstants.VertexPointRadius, 2);
        }

        public bool TrySelect(PointF point, out IPolygonShape? selectedShape)
        {
            if (HitTest(point))
            {
                isSelected = true;
                selectedShape = this;
                return true;
            }

            selectedShape = null;
            return false;
        }


        public void MoveTo(PointF point, bool userMove = false)
        {
            if (locked)
                return;

            X = point.X;
            Y = point.Y;

            locked = userMove ? true : false;
            OnVertexMove?.Invoke(this);
            locked = false;
        }

        public void Move(float dx, float dy, bool silentMove = false)
        {
            if (locked)
                return;

            X += dx;
            Y += dy;

            if (!silentMove)
                OnVertexMove?.Invoke(this);
        }

        public void Deselect()
        {
            isSelected = false;
        }

        public void Delete()
        {
            OnVertexDelete?.Invoke(this, edges);
        }

        public void AttachEdge(Edge edge)
        {
            OnVertexMove += edge.OnVertexMoveHandler;
            edges.Add(edge);
        }

        public void DeattachEdge(Edge edge)
        {
            edges.Remove(edge);
            OnVertexMove -= edge.OnVertexMoveHandler;
        }

        public Edge GetSecondEdge(Edge edge)
        {
            return edges[0] != edge ? edges[0] : edges[1];
        }

        public void Visit(IPolygonVisitor visitor)
        {
            visitor.AcceptVisit(this);
        }
    }
}