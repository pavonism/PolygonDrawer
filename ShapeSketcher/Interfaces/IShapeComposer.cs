
namespace ShapeSketcher
{
    /// <summary>
    /// Interfejs wykorzystywany przez <seealso cref="ShapeSketcher"/> do tworzenia nowych kształtów.
    /// </summary>
    /// <typeparam name="ShapeType"> 
    /// Typ zwracancych przez interfejs kształtów 
    /// </typeparam>
    public interface IShapeComposer<ShapeType> : ICanvasManager where ShapeType : IRenderableShape
    {
        event Action<ShapeType> OnShapeCreation;
        event Action OnShapeCancel;
    }
}
