using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonDrawer
{
    public static class Resources
    {
        public const string ProgramTitle = "PolygonDrawer";

        public const string DrawingModeGlyph = "▲";
        public const string DrawingModeText = "Tryb rysowania";
        public const string PerpendicularModeText = "Relacja prostopadłości";
        public const string LengthConstraintText = "Ograniczenie długości";

        public const string BresenhamOptionText = "Bresenham";

    }

    public static class FormConstants
    {
        public const int MinimumWindowSizeX = 700;
        public const int MinimumWindowSizeY = 500;

        public const int InitialWindowSizeX = 900;
        public const int InitialWindowSizeY = 600;

        public const int MainFormRowCount = 2;
        public const int MinimumControlSize = 32;
    }
}
