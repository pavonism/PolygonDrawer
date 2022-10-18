using Polygons.Shapes;
using ShapeSketcher;
using System.Drawing;
using System.Windows.Forms;

namespace Polygons
{
    public class PolygonComposer : IShapeComposer<IPolygonShape>
    {
        private Polygon? currentPolygon;

        public event Action<IPolygonShape>? OnShapeCreation;
        public event Action? OnShapeCancel;

        public void MouseDown(PointF point, MouseButtons button)
        {
            if (button != MouseButtons.Left)
            {
                currentPolygon = null;
                OnShapeCancel?.Invoke();
                return;
            }

            if (currentPolygon == null)
            {
                currentPolygon = new Polygon();
                currentPolygon.SetStart(point);
                return;
            }

            if (currentPolygon.AddEdgeTo(point))
            {
                if (currentPolygon.VertexCount > 2)
                    OnShapeCreation?.Invoke(currentPolygon);
                else
                {
                    currentPolygon = null;
                    OnShapeCancel?.Invoke();
                }
                currentPolygon = null;
            }
        }

        public void MouseMove(PointF currentPosition)
        {
            currentPolygon?.SetEnd(currentPosition);
        }

        public void MouseClick(PointF from, MouseButtons button)
        {
        }

        public void MouseDrag(PointF from, PointF to, MouseButtons button)
        {
        }

        public void Refresh(Bitmap drawingCanvas, RenderMode renderMode)
        {
            currentPolygon?.Render(drawingCanvas, renderMode);
        }
    }
}
