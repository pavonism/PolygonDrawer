using Polygons.Shapes;
using Polygons.Visitors;

namespace Polygons.Constraints
{
    public interface IConstraint
    {
        string Symbol { get; }
        object Id { get; }
        void Check(Vertex movedVertex, Edge movedEdge);
        void Delete(Edge request);
    }

    public interface IConstraintBuilder : IPolygonVisitor
    {
        public event Action<IConstraint> OnConstraintCreation;
    }
}
