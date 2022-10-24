using Polygons.Shapes;
using Polygons.Visitors;
using System.Data;

namespace Polygons.Constraints
{
    public class RelationBuilder : PolygonVisitor
    {
        private readonly List<Edge> edges = new List<Edge>();
        private int relationCounter = 0;

        public override void AcceptVisit(Edge edge)
        {
            edges.Add(edge);

            if(edges.Count == 2)
            {
                new PerpendicularRelation(edges[0], edges[1], relationCounter++);
                edges.Clear();
            }
        }

        public void Clear()
        {
            this.relationCounter = 0;
        }
    }
}
