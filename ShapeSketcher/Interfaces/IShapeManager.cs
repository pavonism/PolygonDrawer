
namespace ShapeSketcher
{
    /// <summary>
    /// Interfejs obsługujący zarządzanie narysowanymi kształtami. 
    /// </summary>
    public interface IShapeManager<ShapeType> : ICanvasManager  where ShapeType : IRenderableShape
    {
        public IEnumerable<ShapeType> Shapes { get; }
        public ShapeType? Selection { get; }

        public event Action<ShapeType>? OnSelectionChanged;

        public void AddShape(ShapeType shape);
        public void Clear();
    }
}
