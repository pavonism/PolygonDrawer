using Polygons.Visitors;
using ShapeSketcher;
using System.Drawing;

namespace Polygons.Shapes
{
    public class Vertex : IPolygonShape
    {
        #region Fields and Properties
        public float X { get; private set; }
        public float Y { get; private set; }
        /// <summary>
        /// Określa, czy wierzchołek jest zaznaczony
        /// </summary>
        private bool isSelected;
        /// <summary>
        /// Określa, czy wierzchołek może być przemieszczany w ramach sprawdzania ograniczeń krawędzi
        /// </summary>
        public bool Locked { get; set; }
        /// <summary>
        /// Aktualne współrzędne wierzchołka
        /// </summary>
        public PointF Location => new(X, Y);
        /// <summary>
        /// Lista krawędzi, do których należy wierzchołek
        /// </summary>
        private readonly List<Edge> edges = new();
        #endregion

        #region Events
        public event Action<IPolygonShape> OnShapeDelete;
        public event OnVertexDeleteHandler? OnVertexDelete;
        public event Action<Vertex> OnVertexMove;
        #endregion

        #region Constructors
        public Vertex(PointF point)
        {
            X = point.X;
            Y = point.Y;
        }
        #endregion

        #region IPolygonShape
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
            if (Locked)
                return;

            X = point.X;
            Y = point.Y;

            Locked = userMove;
            OnVertexMove?.Invoke(this);
            Locked = false;
        }

        public void Deselect()
        {
            isSelected = false;
        }

        public void Delete()
        {
            OnVertexDelete?.Invoke(this, edges);
        }

        public void Visit(IPolygonVisitor visitor)
        {
            visitor.AcceptVisit(this);
        }
        #endregion

        #region Public Methods
        public bool HitTest(PointF point)
        {
            return Math.Pow(Math.Abs(X - point.X), 2) + Math.Pow(Math.Abs(Y - point.Y), 2) <= Math.Pow(ShapesConstants.VertexPointRadius, 2);
        }

        public void Move(float dx, float dy, bool silentMove = false, bool userMove = false)
        {
            if (Locked)
                return;

            X += dx;
            Y += dy;

            Locked = userMove;
            if (!silentMove)
                OnVertexMove?.Invoke(this);
            Locked = false;
        }

        /// <summary>
        /// Łączy wierzhołek z krawędzią. Dzięki temu możliwe jest sprawdzenie ograniczeń krawędzi, gdy wierzchołek się przesuwa
        /// </summary>
        public void AttachEdge(Edge edge)
        {
            OnVertexMove += edge.OnVertexMoveHandler;
            edges.Add(edge);
        }

        /// <summary>
        /// Usuwa połączenie wierzchołka z krawędzią. Krawędź nie sprawdza już ograniczeń po przemieszczeniu się wierzchołka
        /// </summary>
        /// <param name="edge"></param>
        public void DeattachEdge(Edge edge)
        {
            edges.Remove(edge);
            OnVertexMove -= edge.OnVertexMoveHandler;
        }

        public Edge GetSecondEdge(Edge edge)
        {
            return edges[0] != edge ? edges[0] : edges[1];
        }
        #endregion
    }
}