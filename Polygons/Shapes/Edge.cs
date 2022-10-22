using Polygons.Constraints;
using Polygons.Visitors;
using ShapeSketcher;
using System.Drawing;
using System.Text;

namespace Polygons.Shapes
{
    public class Edge : IPolygonShape
    {
        private readonly HashSet<IConstraint> constraints = new();
        private PointF? selectionPoint;

        private Vertex FirstBezierPoint;
        private Vertex SecondBezierPoint;

        private bool isBezier;
        public bool IsBezier
        {
            get => isBezier;
            set
            {
                if(value && FirstBezierPoint == null && SecondBezierPoint == null)
                {
                    FirstBezierPoint = new Vertex(new PointF(From.X + 10, From.Y + 10));
                    FirstBezierPoint.IsConstructionPoint = true;
                    SecondBezierPoint = new Vertex(new PointF(To.X + 10, To.Y + 10));
                    SecondBezierPoint.IsConstructionPoint = true;
                }

                isBezier = value;
            }
        } 

        public HashSet<IConstraint> Constraints => constraints;

        public float Length
        {
            get => (float)Math.Sqrt(Math.Pow(To.X - From.X, 2) + Math.Pow(To.Y - From.Y, 2));
            set => LengthConstraint.AdjustEdgeLength(this, value);
        }

        private float? fixedLength;
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

        public PointF Center => new PointF((To.X + From.X) / 2, (To.Y + From.Y) / 2);

        private bool isSelected;
        public bool ConstraintLock { get; set; }
        public event Action<IPolygonShape> OnShapeDelete;
        public event OnEdgeDelete? OnEdgeDelete;
        public event Action<Edge, PointF>? OnVertexAdd;

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

        public void Render(Bitmap drawingContext, RenderMode renderMode)
        {
            if(IsBezier)
            {
                Bezier.Draw(new PointF[] { From.Location, FirstBezierPoint.Location, SecondBezierPoint.Location, To.Location }, drawingContext, ShapesConstants.DefaultLineColor);
                FirstBezierPoint.Render(drawingContext, renderMode);
                SecondBezierPoint.Render(drawingContext, renderMode);
                return;
            }

            switch (renderMode)
            {
                case RenderMode.Default:
                    RenderDefault(drawingContext);
                    break;
                case RenderMode.Bresenham:
                    if (isSelected)
                        XiaolinWu.DrawLine((int)From.X, (int)From.Y, (int)To.X, (int)To.Y, drawingContext, ShapesConstants.SelectionLineColor);
                    else
                        XiaolinWu.DrawLine((int)From.X, (int)From.Y, (int)To.X, (int)To.Y, drawingContext, ShapesConstants.DefaultLineColor);
                    break;
            }

            RenderConstraintSymbols(drawingContext);
        }

        private void RenderDefault(Bitmap drawingContext)
        {
            using (Graphics g = Graphics.FromImage(drawingContext))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

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

                foreach (var constraint in constraints)
                {
                    symbols.Add(constraint.Id + constraint.Symbol);
                }

                g.DrawString(string.Join(" ", symbols), new Font("Arial", 8), Brushes.Black, Center);
            }
        }

        public bool HitTest(PointF point)
        {
            var ab = Math.Pow(To.X - From.X, 2) + Math.Pow(To.Y - From.Y, 2);
            var ac = Math.Pow(point.X - From.X, 2) + Math.Pow(point.Y - From.Y, 2);
            var cb = Math.Pow(To.X - point.X, 2) + Math.Pow(To.Y - point.Y, 2);

            return Math.Abs(Math.Sqrt(ab) - Math.Sqrt(ac) - Math.Sqrt(cb)) < ShapesConstants.EdgeSelectionRadius;
        }

        private bool locked;

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
            if(IsBezier && (FirstBezierPoint.TrySelect(point, out selectedShape) || SecondBezierPoint.TrySelect(point, out selectedShape)))
                return true;

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

        internal void OnVertexMoveHandler(Vertex vertex)
        {
            if (ConstraintLock)
                return;

            CheckConstraints(vertex);
        }

        public void AddConstraint(IConstraint constraint)
        {
            constraints.Add(constraint);
        }

        public void RemoveConstraint(IConstraint constraint)
        {
            constraints.Remove(constraint);
        }

        public Vertex GetSecondVertex(Vertex vertex)
        {
            return To != vertex ? To : From;
        }

        public void Visit(IPolygonVisitor visitor)
        {
            visitor.AcceptVisit(this);
        }

        public void AddNewVertex(PointF point)
        {
            this.OnVertexAdd?.Invoke(this, point);
        }

        public void CheckConstraints(Vertex movedVertex)
        {
            foreach (var constraint in constraints)
            {
                constraint.Check(movedVertex, this);
            }
        }

        public void ClearConstraints()
        {
            foreach (var constraint in constraints)
            {
                constraint.Delete(this);
            }

            constraints.Clear();
        }
    }
}