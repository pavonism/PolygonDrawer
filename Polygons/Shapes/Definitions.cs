using System.Drawing;

namespace Polygons.Shapes
{
    #region Event Handlers
    public delegate void OnVertexDeleteHandler(Vertex vertex, List<Edge> edges);
    public delegate void OnEdgeDelete(Edge edge);
    #endregion

    public static class ShapesConstants
    {
        public const string EdgeFixedLengthSymbol = "‡";
        public const float EdgeSelectionRadius = 0.5f;
        public const float VertexPointRadius = 5;

        public static Pen SelectionPen = Pens.Red;
        public static Brush SelectionBrush = Brushes.Red;

        public static Color DefaultLineColor = Color.Black;
        public static Color SelectionLineColor = Color.Red;
        public static Pen DefaultPen = Pens.Black;
        public static Brush DefaultBrush = Brushes.Black;
    }
}
