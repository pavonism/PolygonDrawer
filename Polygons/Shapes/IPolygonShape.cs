using Polygons.Visitors;
using ShapeSketcher;
using System.Drawing;

namespace Polygons.Shapes
{
    public interface IPolygonShape : IRenderableShape
    {
        /// <summary>
        /// Zdarzenie generowane w momencie usunięcia kształtu.
        /// </summary>
        event Action<IPolygonShape> OnShapeDelete;
        /// <summary>
        /// Próbuje zaznaczyć kształt (lub jeden z jego podkształtów występujących w punkcie <paramref name="point"/>. 
        /// </summary>
        /// <param name="point"> Punkt, w którym następuje próba zaznaczenia </param>
        /// <param name="selectedShape"> Kształt (lub podkształt), który udało się zaznaczyć </param>
        /// <returns> Informacja, czy udało się zaznaczyć kształt (lub jakiś podształt) </returns>
        bool TrySelect(PointF point, out IPolygonShape? selectedShape);
        /// <summary>
        /// Wyłącza zaznaczenie kształtu
        /// </summary>
        void Deselect();
        /// <summary>
        /// Implementacja wzorca projektowego wizytor. 
        /// Umożliwia wywołanie innej metody dla danego typu obiektu z kolekcji obiektów wspólnego interfejsu
        /// </summary>
        void Visit(IPolygonVisitor visitor);
    }
}
