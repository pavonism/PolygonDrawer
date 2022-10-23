using Polygons.Constraints;
using Polygons.Visitors;
using ShapeSketcher;
using System.Drawing;
using System.Text;

namespace Polygons.Shapes
{
    public class Edge : IPolygonShape
    {
        #region Fields and Properties
        /// <summary>
        /// Zbiór wszystkich ograniczeń nałożonych na krawędź
        /// </summary>
        public HashSet<IConstraint> Constraints { get; private set; } = new();
        /// <summary>
        /// Punkt, w których zaznaczono krawędź, względem niego odbywają się przesunięcia
        /// </summary>
        private PointF? selectionPoint;
        /// <summary>
        /// Określa, czy krawędź jest zaznaczona. Zaznaczona krawędź podświetla się na czerwono
        /// </summary>
        private bool isSelected;
        /// <summary>
        /// Określa, czy krawędź może być przemieszczana 
        /// </summary>
        private bool locked;
        /// <summary>
        /// Długość krawędzi
        /// </summary>
        public float Length
        {
            get => (float)Math.Sqrt(Math.Pow(To.X - From.X, 2) + Math.Pow(To.Y - From.Y, 2));
            set => LengthConstraint.AdjustEdgeLength(this, value);
        }
        /// <summary>
        /// Środek krawędzi
        /// </summary>
        public PointF Center => new PointF((To.X + From.X) / 2, (To.Y + From.Y) / 2);

        /// <summary>
        /// Określa, czy powinny być sprawdzane ograniczenia krawędzi
        /// </summary>
        public bool ConstraintLock { get; set; }

        private float? fixedLength;
        /// <summary>
        /// Stała długość krawędzi. Wiąże się z dodaniem ograniczenia na długość
        /// </summary>
        public float? FixedLength
        {
            get => fixedLength.HasValue ? fixedLength : null;
            set
            {
                fixedLength = value;

                if (value.HasValue)
                    LengthConstraint.AdjustEdgeLength(this, value.Value);
            }
        }

        private Vertex from;
        /// <summary>
        /// Pierwszy koniec krawędzi
        /// </summary>
        public Vertex From
        {
            get => from;
            set
            {
                from?.DeattachEdge(this);
                from = value;
                from?.AttachEdge(this);
                CheckConstraints(From);
            }
        }

        private Vertex to;
        /// <summary>
        /// Drugi koniec krawędzi
        /// </summary>
        public Vertex To
        {
            get => to;
            set
            {
                to?.DeattachEdge(this);
                to = value;
                to?.AttachEdge(this);
                CheckConstraints(To);
            }
        }
        #endregion

        #region Events and Handlers
        public event Action<IPolygonShape>? OnShapeDelete;
        public event OnEdgeDelete? OnEdgeDelete;
        public event Action<Edge, PointF>? OnVertexAdd;

        internal void OnVertexMoveHandler(Vertex vertex)
        {
            if (ConstraintLock)
                return;

            CheckConstraints(vertex);
        }
        #endregion

        #region Constructors
        public Edge(Vertex from, Vertex to)
        {
            From = from;
            To = to;
        }

        public Edge(Edge source)
        {
            From = source.From;
            To = source.To;
        }
        #endregion

        #region Constraints
        public void AddConstraint(IConstraint constraint)
        {
            Constraints.Add(constraint);
        }

        public void RemoveConstraint(IConstraint constraint)
        {
            Constraints.Remove(constraint);
        }

        public void CheckConstraints(Vertex movedVertex)
        {
            foreach (var constraint in Constraints)
            {
                constraint.Check(movedVertex, this);
            }
        }

        public void ClearConstraints()
        {
            foreach (var constraint in Constraints)
            {
                constraint.Delete(this);
            }

            Constraints.Clear();
        }
        #endregion

        #region IPolygonShape 
        public void Render(Bitmap drawingContext, RenderMode renderMode)
        {
            switch (renderMode)
            {
                case RenderMode.Default:
                    RenderDefault(drawingContext);
                    break;
                case RenderMode.Bresenham:
                    if (isSelected)
                        Bresenham.DrawLine((int)From.X, (int)From.Y, (int)To.X, (int)To.Y, drawingContext, ShapesConstants.SelectionLineColor);
                    else
                        Bresenham.DrawLine((int)From.X, (int)From.Y, (int)To.X, (int)To.Y, drawingContext, ShapesConstants.DefaultLineColor);
                    break;
            }

            RenderConstraintSymbols(drawingContext);
        }

        private void RenderDefault(Bitmap drawingContext)
        {
            using (Graphics g = Graphics.FromImage(drawingContext))
            {
                if (isSelected)
                    g.DrawLine(ShapesConstants.SelectionPen, From.X, From.Y, To.X, To.Y);
                else
                    g.DrawLine(ShapesConstants.DefaultPen, From.X, From.Y, To.X, To.Y);
            }
        }

        private void RenderConstraintSymbols(Bitmap drawingContext)
        {
            using (Graphics g = Graphics.FromImage(drawingContext))
            {
                List<string> symbols = new();

                foreach (var constraint in Constraints)
                {
                    symbols.Add(constraint.Id + constraint.Symbol);
                }

                g.DrawString(string.Join(" ", symbols), new Font("Arial", 8), Brushes.Black, Center);
            }
        }

        public void MoveTo(PointF point, bool userMove = false)
        {
            if (locked)
                return;

            if (selectionPoint.HasValue)
            {
                float dx = point.X - selectionPoint.Value.X;
                float dy = point.Y - selectionPoint.Value.Y;
                selectionPoint = point;

                locked = userMove;
                To.Locked = true;
                From.Move(dx, dy, false, true);
                To.Locked = false;
                From.Locked = true;
                To.Move(dx, dy, false, true);
                From.Locked = false;
                locked = false;
                //ConstraintLock = false;
            }
        }

        public bool TrySelect(PointF point, out IPolygonShape? selectedShape)
        {
            if (HitTest(point))
            {
                isSelected = true;
                selectionPoint = point;
                selectedShape = this;
                return true;
            }
            selectionPoint = null;
            selectedShape = null;
            return false;
        }

        public void Deselect()
        {
            isSelected = false;
        }

        public void Delete()
        {
            ClearConstraints();
            OnEdgeDelete?.Invoke(this);
        }

        public void Visit(IPolygonVisitor visitor)
        {
            visitor.AcceptVisit(this);
        }
        #endregion

        #region Public Methods
        public bool HitTest(PointF point)
        {
            var ab = Math.Pow(To.X - From.X, 2) + Math.Pow(To.Y - From.Y, 2);
            var ac = Math.Pow(point.X - From.X, 2) + Math.Pow(point.Y - From.Y, 2);
            var cb = Math.Pow(To.X - point.X, 2) + Math.Pow(To.Y - point.Y, 2);

            return Math.Abs(Math.Sqrt(ab) - Math.Sqrt(ac) - Math.Sqrt(cb)) < ShapesConstants.EdgeSelectionRadius;
        }

        public void AddNewVertex(PointF point)
        {
            this.OnVertexAdd?.Invoke(this, point);
        }

        public Vertex GetSecondVertex(Vertex vertex)
        {
            return To != vertex ? To : From;
        }
        #endregion
    }
}