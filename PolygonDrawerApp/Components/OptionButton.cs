using System.Drawing.Drawing2D;

namespace PolygonDrawerApp.Components
{
    /// <summary>
    /// Implementuje okrągły przycisk w formie checkboxa
    /// </summary>
    public class OptionButton : Button
    {
        #region Fields and Properties
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
        #endregion

        #region Events
        public event Action<bool>? OnOptionChanged;

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
        #endregion

        public OptionButton()
        {
            BackColor = Color.Transparent;
            ForeColor = Color.Black;
            FlatStyle = FlatStyle.Flat;
            this.Click += OptionChanged;
        }
    }
}
