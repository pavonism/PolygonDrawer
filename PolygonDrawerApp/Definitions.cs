
namespace PolygonDrawer
{
    public static class Resources
    {
        public const string ProgramTitle = "PolygonDrawer  \u25B2";
        public const string ClearGlyph = "\U0001F5D1";
        public const string ReloadDemoGlyph = "\u21BB";

        public const string ClearTextHint = "Wyczyść rysunek.";
        public const string ReloadDemoHint = "Załaduj rysunek demonstracyjny.";
        public const string PerpendicularModeHint = "Relacja prostopadłości. \nAby nadać relację, kliknij LPM na dwie krawędzie.";
        public const string LengthConstraintHint = "Ograniczenie długości. \nKliknij, aby zablokować/odblokować długośc krawędzi.";

        public const string BresenhamOptionText = "Bresenham";
        public const string BresenhamTooltip = "Włącza renderowanie linii za pomocą algorytmu Bresenhama.";
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
