
namespace ShapeSketcher
{
    public interface IShapeManager<ShapeType> : ICanvasManager  where ShapeType : IRenderableShape
    {
        public IEnumerable<ShapeType> Shapes { get; }
        public ShapeType? Selection { get; }

        public void AddShape(ShapeType shape);
        public event Action<ShapeType>? OnSelectionChanged;
    }
}
