
namespace ShapeSketcher
{
    public delegate void OnShapeSelection(IShape shape);
    public delegate void OnShapeDelete(IRenderableShape shape);


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