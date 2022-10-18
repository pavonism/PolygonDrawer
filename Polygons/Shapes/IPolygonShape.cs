using Polygons.Visitors;
using ShapeSketcher;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygons.Shapes
{
    public interface IPolygonShape : IRenderableShape
    {
        event Action<IPolygonShape> OnShapeDelete;

        bool TrySelect(PointF point, out IPolygonShape? selectedShape);
        void Deselect();
        void Visit(IPolygonVisitor visitor);
    }
}
