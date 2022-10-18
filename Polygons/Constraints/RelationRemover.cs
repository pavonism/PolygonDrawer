using Polygons.Shapes;

namespace Polygons.Constraints
{
    public class RelationBuilder : IConstraintBuilder
    {
        private readonly List<Edge> edges = new List<Edge>();
        private int relationCounter = 0;
        public event Action<IConstraint> OnConstraintCreation;

        public void AcceptVisit(Edge edge)
        {
            edges.Add(edge);

            if(edges.Count == 2)
            {
                new PerpendicularRelation(edges[0], edges[1], relationCounter++);
                edges.Clear();
            }
        }

        public void AcceptVisit(Polygon polygon)
        {
        }

        public void AcceptVisit(Vertex vertex)
        {
        }
    }
}
