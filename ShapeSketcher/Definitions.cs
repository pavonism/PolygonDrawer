
namespace ShapeSketcher
{
    public static class ControlConstants
    {
        public const int CanvasDefaultWidth = 1000;
        public const int CanvasDefaultHeight = 1000;
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
