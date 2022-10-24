
namespace ShapeSketcher
{
    public delegate void OnShapeSelection(IShape shape);
    public delegate void OnShapeDelete(IRenderableShape shape);

    /// <summary>
    /// Podstawowy interfejs kształtów, na których operuje ShapeSketcher. 
    /// Jego przeciążenie umożliwia tworzenie oraz zarządzanie własnymi kształtami.
    /// </summary>
    public interface IRenderableShape : IShape
    {
        void Render(Bitmap drawingCanvas, RenderMode renderMode);
    }

    public interface IShape
    {
        public bool HitTest(PointF point);
        void MoveTo(PointF point, bool userMove);
        void Delete();
    }

    public enum ShapeMode
    {
        Editing, 
        Drawing,
    }
}