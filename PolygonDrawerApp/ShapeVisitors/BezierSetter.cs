using Polygons.Shapes;
using Polygons.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonDrawerApp.ShapeVisitors
{
    public class BezierSetter : PolygonVisitor
    {
        public override void AcceptVisit(Edge edge)
        {
            edge.IsBezier = !edge.IsBezier;
        }
    }
}
