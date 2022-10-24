using Polygons;
using Polygons.Constraints;
using Polygons.Shapes;

namespace PolygonDrawer.Demo
{
    public class DemoScene : IDemoScene
    {
        public void Load(PolygonManager manager)
        {
            // Pierwszy wielokąt
            Polygon newPolygon = new();
            newPolygon.SetStart(new PointF(100, 100));
            newPolygon.AddEdgeTo(new PointF(120, 200), out var constLengthEdge1);
            newPolygon.AddEdgeTo(new PointF(200, 230));
            newPolygon.AddEdgeTo(new PointF(300, 150), out var perpendEdge1);
            newPolygon.AddEdgeTo(new PointF(200, 120));
            newPolygon.AddEdgeTo(new PointF(150, 140), out var perpendEdge3);
            newPolygon.AddEdgeTo(new PointF(100, 100), out var perpendEdge2);

            perpendEdge1.Visit(manager.RelationBuilder);
            perpendEdge2.Visit(manager.RelationBuilder);
            perpendEdge1.FixedLength = 120;
            perpendEdge1.AddConstraint(LengthConstraint.Instance);
            constLengthEdge1.FixedLength = 180;
            constLengthEdge1.AddConstraint(LengthConstraint.Instance);

            manager.AddShape(newPolygon);

            // Drugi wielokąt
            newPolygon = new();
            newPolygon.SetStart(new PointF(600, 400));
            newPolygon.AddEdgeTo(new PointF(700, 450), out var constLengthEdge2);
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
            constLengthEdge2.FixedLength = 150;
            constLengthEdge2.AddConstraint(LengthConstraint.Instance);

            manager.AddShape(newPolygon);
        }
    }
}
