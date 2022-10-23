using Polygons.Shapes;
using Polygons.Visitors;

namespace Polygons.Constraints
{
    /// <summary>
    /// Interfejs definiujący wszystkie ograniczenia.
    /// </summary>
    public interface IConstraint
    {
        /// <summary>
        /// Symbol wyświetlany nad krawędzią, na którą zostało nałożone ogranicznie
        /// </summary>
        string Symbol { get; }
        /// <summary>
        /// Id ograniczenia. Wykorzystywane, aby odróżnić między sobą relacje tego samego typu
        /// </summary>
        object Id { get; }
        /// <summary>
        /// Metoda sprawdzająca ograniczenie
        /// </summary>
        /// <param name="movedVertex"> Wierzchołek, którego poruszenie wymusiło ponowne przeliczenie ograniczenia </param>
        /// <param name="movedEdge"> Krawędź, do której należy <paramref name="movedVertex"/>. </param>
        void Check(Vertex movedVertex, Edge movedEdge);
        /// <summary>
        /// Metoda usuwająca zadane ograniczenie (wykorzystywane głównie przy mechanizmie relacji).
        /// </summary>
        void Delete(Edge request);
    }

    public interface IConstraintBuilder : IPolygonVisitor
    {
        public event Action<IConstraint> OnConstraintCreation;
    }
}
