using Polygons.Shapes;
using Polygons.Visitors;
using System.Security.Cryptography;

namespace Polygons.Constraints
{
    public class RelationRemover : PolygonVisitor
    {
        private HashSet<IConstraint>? constraints;
        private int relationCounter = 0;

        public override void AcceptVisit(Edge edge)
        {
            if(this.relationCounter == 0)
            {
                this.constraints = edge.Constraints;
                relationCounter++;
            }
            else if(this.constraints != null)
            {
                this.relationCounter = 0;
                HashSet<IConstraint> toRemove = new();

                foreach (var constraint in edge.Constraints)
                {
                    if (constraints.Contains(constraint))
                        toRemove.Add(constraint);
                }

                foreach (var constraint in toRemove)
                {
                    constraints.Remove(constraint);
                    edge.Constraints.Remove(constraint);
                }
            }
        }
    }
}
