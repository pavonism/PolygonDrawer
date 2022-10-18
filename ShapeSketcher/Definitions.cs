
namespace ShapeSketcher
{
    public static class ControlConstants
    {
        public const int CanvasWidth = 1000;
        public const int CanvasHeight = 1000;
    }

    public enum ControlMode
    {
        Select,
        Drawing,
    }

    public enum RenderMode
    {
        Default,
        Bresenham,
    }
}
