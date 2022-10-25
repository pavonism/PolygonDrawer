using ShapeSketcher;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygons.Shapes
{
    public class ConstructionPoint : Vertex
    {
        public ConstructionPoint(PointF point) : base(point)
        {
        }

        public override void Render(Bitmap drawingContext, RenderMode renderMode)
        {
            RectangleF rectangle = new RectangleF(X - ShapesConstants.VertexPointRadius, Y - ShapesConstants.VertexPointRadius,
                2 * ShapesConstants.VertexPointRadius, 2 * ShapesConstants.VertexPointRadius);

            using (var g = Graphics.FromImage(drawingContext))
            {
                g.FillRectangles(ShapesConstants.ConstructionBrush,  new RectangleF[] { rectangle });
            }
        }
    }
}
