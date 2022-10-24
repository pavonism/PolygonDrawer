using System.Drawing;
using Polygons.Visitors;
using ShapeSketcher;

namespace Polygons.Shapes
{
    public class Polygon : IPolygonShape
    {
        #region Fields and Properties
        private readonly HashSet<Edge> edges = new();
        private readonly HashSet<Vertex> vertices = new();

        private ShapeMode shapeMode = ShapeMode.Drawing;
        private Vertex? firstPoint;
        private Edge? drawingEdge;
        private PointF? selectionPoint;

        public int VertexCount => vertices.Count;
        private IEnumerable<IPolygonShape> AllComponents => vertices.Concat<IPolygonShape>(edges);
        #endregion

        #region Events and Handlers
        public event Action<IPolygonShape>? OnShapeDelete;

        /// <summary>
        /// Obsługuje zdarzenie dodania nowego wierzchołka na krawędzi
        /// </summary>
        private void VertexAddHandler(Edge edge, PointF point)
        {
            var newVertex = CreateVertex(point);
            vertices.Add(newVertex);
            edges.Add(CreateEdge(edge.From, newVertex));
            edge.ClearConstraints();
            edge.From = newVertex;
        }

        /// <summary>
        /// Obsługuje zdarzenie usunięcia krawędzi
        /// </summary>
        /// <param name="edge"></param>
        private void EdgeDeleteHandler(Edge edge)
        {
            var newVertex = CreateVertex(edge.Center);
            vertices.Add(newVertex);
            DeletePartOfEdge(edge, edge.From, newVertex);
            DeletePartOfEdge(edge, edge.To, newVertex);

            edges.Remove(edge);
            if (edges.Count < 3)
                Delete();
        }

        private void DeletePartOfEdge(Edge edgeToRemove, Vertex vertex, Vertex newVertex)
        {
            var edge = vertex.GetSecondEdge(edgeToRemove);

            if (edge.To == vertex) edge.To = newVertex;
            else edge.From = newVertex;

            vertices.Remove(vertex);
        }

        /// <summary>
        /// Obsługuje zdarzenie usunięcia wierzchołka
        /// </summary>
        private void VertexDeleteHandler(Vertex vertex, List<Edge> edges)
        {
            if (edges.Count == 2)
            {
                Vertex[] neighboringVertices = new Vertex[2];

                for (int i = 0; i < edges.Count; i++)
                {
                    neighboringVertices[i] = edges[i].To != vertex ? edges[i].To : edges[i].From;
                    neighboringVertices[i].DeattachEdge(edges[i]);
                }

                edges.ForEach((edge) =>
                {
                    edge.ClearConstraints();
                    this.edges.Remove(edge);
                });

                CreateEdge(neighboringVertices[0], neighboringVertices[1]);
                vertices.Remove(vertex);

                if (vertices.Count < 3)
                    Delete();
            }
        }
        #endregion

        #region Drawing
        /// <summary>
        /// Ustawia pierwszy wierzchołek wielokąta
        /// </summary>
        public void SetStart(PointF point)
        {
            drawingEdge = CreateEdge(CreateVertex(point), CreateVertex(point));
            if (firstPoint == null)
                firstPoint = drawingEdge.From;
            vertices.Add(drawingEdge.From);
        }

        /// <summary>
        /// Dodaje krawędź do wielokąta.
        /// </summary>
        /// <param name="point"> 
        /// Punkt końcowy dla bieżąco rysowanej krawędzi 
        /// </param>
        /// <returns>
        /// Jeżeli wielokąt został domknięty i proces tworzenia wielokąta
        /// się zakończył, to zwraca true, inaczej zwraca false
        /// </returns>
        public bool AddEdgeTo(PointF point)
        {
            return AddEdgeTo(point, out _);
        }

        public bool AddEdgeTo(PointF point, out Edge createdEdge)
        {
            createdEdge = drawingEdge ?? throw new NullReferenceException();

            if (firstPoint != null && firstPoint.HitTest(point))
            {
                drawingEdge.To = firstPoint;
                shapeMode = ShapeMode.Editing;
                return true;
            }
            else
            {
                drawingEdge.To = CreateVertex(point);
                vertices.Add(drawingEdge.To);
                drawingEdge = CreateEdge(drawingEdge.To, CreateVertex(point));
                return false;
            }
        }

        /// <summary>
        /// Ustawia koniec bieżąco rysowanej krawędzi
        /// </summary>
        internal void SetEnd(PointF point)
        {
            drawingEdge?.To.MoveTo(point);
        }

        private Vertex CreateVertex(PointF point)
        {
            var newVertex = new Vertex(point);
            newVertex.OnVertexDelete += VertexDeleteHandler;
            return newVertex;
        }

        private Edge CreateEdge(Vertex from, Vertex to)
        {
            var newEdge = new Edge(from, to);
            newEdge.OnEdgeDelete += EdgeDeleteHandler;
            newEdge.OnVertexAdd += VertexAddHandler;
            edges.Add(newEdge);
            return newEdge;
        }
        #endregion

        #region Public Methods
        public void GetExtremePoints(out PointF minPoint, out PointF maxPoint)
        {
            minPoint = new(float.MaxValue, float.MaxValue);
            maxPoint = new(float.MinValue, float.MinValue);

            foreach (var vertex in vertices)
            {
                minPoint.X = Math.Min(vertex.X, minPoint.X);
                minPoint.Y = Math.Min(vertex.Y, minPoint.Y);
                maxPoint.X = Math.Max(vertex.X, maxPoint.X);
                maxPoint.Y = Math.Max(vertex.Y, maxPoint.Y);
            }
        }

        public bool HitTest(PointF point)
        {
            GetExtremePoints(out var minPoint, out var maxPoint);
            RectangleF rect = new RectangleF(minPoint.X, minPoint.Y, maxPoint.X - minPoint.X, maxPoint.Y - minPoint.Y);
            return rect.Contains(point);
        }
        #endregion

        #region IPolygonShape
        public void Render(Bitmap drawingContext, RenderMode renderMode)
        {
            foreach (var component in edges)
                component.Render(drawingContext, renderMode);

            foreach (var component in vertices)
                component.Render(drawingContext, renderMode);

            if (shapeMode == ShapeMode.Drawing)
                drawingEdge?.Render(drawingContext, renderMode);
        }

        public bool TrySelect(PointF point, out IPolygonShape? selectedShape)
        {
            foreach (var component in AllComponents)
            {
                if (component.TrySelect(point, out var selectedComponent))
                {
                    selectedShape = selectedComponent;
                    return true;
                }
            }

            if (HitTest(point))
            {
                selectionPoint = point;
                selectedShape = this;
                return true;
            }

            selectionPoint = null;
            selectedShape = null;
            return false;
        }

        public void MoveTo(PointF point, bool userMove)
        {
            if (selectionPoint.HasValue)
            {
                float dx = point.X - selectionPoint.Value.X;
                float dy = point.Y - selectionPoint.Value.Y;
                selectionPoint = point;

                foreach (var vertex in vertices)
                {
                    vertex.Move(dx, dy, true);
                }
            }
        }

        public void Deselect()
        {
            selectionPoint = null;
        }

        public void Delete()
        {
            foreach (var edge in edges)
            {
                edge.ClearConstraints();
            }
            OnShapeDelete?.Invoke(this);
        }

        public void Visit(IPolygonVisitor visitor)
        {
            visitor.AcceptVisit(this);
        }
        #endregion
    }
}