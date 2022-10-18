using Polygons.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygons.Visitors
{
    public abstract class PolygonVisitor : IPolygonVisitor
    {
        public virtual void AcceptVisit(Edge edge)
        {
        }

        public virtual void AcceptVisit(Polygon polygon)
        {
        }

        public virtual void AcceptVisit(Vertex vertex)
        {
        }
    }
}
