using System.Drawing.Drawing2D;

namespace PolygonDrawerApp.Components
{
    public class OptionButton : Button
    {
        public event Action<bool>? OnOptionChanged;
        private bool ticked;

        public bool Lock
        {
            get => ticked;
            set
            {
                ticked = value;
                BackColor = value ? Color.LightGray : Color.Transparent;
            }
        }

        public OptionButton()
        {
            this.Click += OptionChanged;
        }

        private void OptionChanged(object? sender, EventArgs e)
        {
            Lock = !Lock;
            OnOptionChanged?.Invoke(ticked);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var ellipseRect = new RectangleF(0, 0, Width, Height);

            using (var path = new GraphicsPath())
            {
                path.AddEllipse(ellipseRect);
                Region = new Region(path);
            }


            base.OnPaint(pevent);
        }
    }
}
