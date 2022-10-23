using Polygons.Constraints;
using Polygons.Shapes;
using Polygons.Visitors;
using ShapeSketcher;
using System.Drawing;
using System.Windows.Forms;

namespace Polygons
{
    public class PolygonManager : IShapeManager<IPolygonShape>
    {
        private readonly HashSet<IPolygonShape> shapes = new HashSet<IPolygonShape>();
        private IPolygonShape? selection;
        public RelationBuilder RelationBuilder { get; } = new RelationBuilder();
        public RelationRemover RelationRemover { get; } = new RelationRemover();

        private bool TrySelectShape(PointF point)
        {
            this.selection?.Deselect();

            foreach (var shape in Shapes)
            {
                if (shape.TrySelect(point, out var selectedShape) && selectedShape != null)
                {
                    this.selection = selectedShape;
                    this.OnSelectionChanged?.Invoke(selectedShape);
                    return true;
                }
            }

            return false;
        }

        private void Select(PointF point, MouseButtons button)
        {
            var lastSelection = Selection;

            if (!TrySelectShape(point))
            {
                selection = null;
            }
            else
            {
                if (button == MouseButtons.Right)
                    selection?.Delete();
            }
        }

        private void TryAddRelation(PointF point)
        {
            if (TrySelectShape(point))
            {
                this.selection?.Visit(RelationBuilder);
            }
        }

        private void TryRemoveRelation(PointF point)
        {
            if (TrySelectShape(point))
            {
                this.selection?.Visit(RelationRemover);
            }
        }

        #region IShapeManager
        public IPolygonShape? Selection => selection;
        public IEnumerable<IPolygonShape> Shapes => shapes;
        public ManagerMode ManagerMode { get; set; }

        public event Action<IPolygonShape>? OnSelectionChanged;

        public void MouseDown(PointF point, MouseButtons button)
        {
            switch (ManagerMode)
            {
                case ManagerMode.Select:
                    Select(point, button);
                    break;
                case ManagerMode.AddRelation:
                    if (button == MouseButtons.Left)
                        TryAddRelation(point);
                    else
                        TryRemoveRelation(point);
                    break;
            }
        }

        public void MouseClick(PointF point, MouseButtons button)
        {
        }

        public void MouseDoubleClick(PointF point, MouseButtons button)
        {
            if (TrySelectShape(point))
            {
                var pointAdder = new PointAdder(point);
                this.selection?.Visit(pointAdder);
            }
        }

        public void MouseDrag(PointF from, PointF to, MouseButtons button)
        {
            if (button != MouseButtons.Left)
                return;

            selection?.MoveTo(to, true);
        }

        public void MouseMove(PointF currentPosition)
        {
        }

        public void Refresh(Bitmap drawingCanvas, RenderMode renderMode)
        {
            foreach (var shape in Shapes)
            {
                shape.Render(drawingCanvas, renderMode);
            }
        }

        public void AddShape(IPolygonShape shape)
        {
            shapes.Add(shape);
            shape.OnShapeDelete += Shape_OnShapeDelete;
        }

        private void Shape_OnShapeDelete(IPolygonShape shape)
        {
            this.shapes.Remove(shape);
        }
        #endregion
    }
}
