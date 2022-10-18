using Polygons;
using Polygons.Constraints;
using Polygons.Shapes;
using ShapeSketcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonDrawerApp.Demo
{
    public class DemoScene : IDemoScene
    {
        public void Load(PolygonManager manager)
        {
            // Pierwszy wielokąt
            Polygon newPolygon = new();
            newPolygon.SetStart(new PointF(100, 100));
            newPolygon.AddEdgeTo(new PointF(120, 200));
            newPolygon.AddEdgeTo(new PointF(200, 230));
            newPolygon.AddEdgeTo(new PointF(300, 150), out var perpendEdge1);
            newPolygon.AddEdgeTo(new PointF(200, 120));
            newPolygon.AddEdgeTo(new PointF(150, 140), out var perpendEdge3);
            newPolygon.AddEdgeTo(new PointF(100, 100), out var perpendEdge2);

            perpendEdge1.Visit(manager.RelationBuilder);
            perpendEdge2.Visit(manager.RelationBuilder);
            perpendEdge1.FixedLength = 120;
            perpendEdge1.AddConstraint(LengthConstraint.Instance);

            manager.AddShape(newPolygon);

            // Drugi wielokąt
            newPolygon = new();
            newPolygon.SetStart(new PointF(600, 400));
            newPolygon.AddEdgeTo(new PointF(700, 450));
            newPolygon.AddEdgeTo(new PointF(750, 350), out var perpendEdge4);
            newPolygon.AddEdgeTo(new PointF(550, 300), out var perpendEdge5);
            newPolygon.AddEdgeTo(new PointF(400, 400));
            newPolygon.AddEdgeTo(new PointF(600, 400));

            perpendEdge4.Visit(manager.RelationBuilder);
            perpendEdge3.Visit(manager.RelationBuilder);
            perpendEdge4.Visit(manager.RelationBuilder);
            perpendEdge5.Visit(manager.RelationBuilder);
            perpendEdge4.FixedLength = 60;
            perpendEdge4.AddConstraint(LengthConstraint.Instance);

            manager.AddShape(newPolygon);
        }
    }
}
