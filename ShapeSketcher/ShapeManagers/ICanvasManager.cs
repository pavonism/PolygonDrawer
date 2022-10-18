
namespace ShapeSketcher
{
    /// <summary>
    /// Interfejs służy do zarządzania kształtami w ShapeSketcher.
    /// </summary>
    public interface ICanvasManager
    {
        void MouseDown(PointF point, MouseButtons button);
        void MouseClick(PointF from, MouseButtons button);
        void MouseMove(PointF currentPosition);
        void MouseDrag(PointF from, PointF to, MouseButtons button);

        void Refresh(Bitmap drawingCanvas, RenderMode renderMode);
    }
}
