
namespace PolygonDrawer.Components
{
    /// <summary>
    /// Umożliwia tworzenie paska z opcjami i guzikami
    /// </summary>
    public class Toolbar : FlowLayoutPanel
    {
        public Toolbar()
        {
            Dock = DockStyle.Fill;
            Padding = Padding.Empty;
            Height = FormConstants.MinimumControlSize;
        }

        public void AddLabel(string text)
        {
            var label = new Label()
            {
                Font = new Font(DefaultFont, FontStyle.Bold),
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = FormConstants.MinimumControlSize,
            };

            label.Width = TextRenderer.MeasureText(text, label.Font).Width;
            Controls.Add(label);
        }

        public void AddDivider()
        {
            Controls.Add(new Divider());
        }

        public void AddButton(EventHandler handler, string glyph, string hint)
        {
            var button = new OptionButton() 
            { 
                Text = glyph,
                Margin = new Padding(2, 0, 2, 0),
            };

            var tooltip = new ToolTip();
            tooltip.SetToolTip(button, hint);
            button.Click += handler;
            Controls.Add(button);
        }

        public CheckButton AddTool(Action<bool> handler, string glyph, string hint)
        {

            var button = new CheckButton()
            {
                Width = FormConstants.MinimumControlSize,
                Height = FormConstants.MinimumControlSize,
                Margin = new Padding(2, 0, 2, 0),
                Text = glyph,
            };

            button.OnOptionChanged += handler;

            var tooltip = new ToolTip();
            tooltip.SetToolTip(button, hint);
            Controls.Add(button);
            return button;
        }

        public CheckBox AddOption(string text, EventHandler onOptionChanged, string? hint = null)
        {
            var checkBox = new CheckBox()
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = text,
            };

            if (hint != null)
            {
                var tooltipControl = new ToolTip();
                tooltipControl.SetToolTip(checkBox, hint);
            }

            checkBox.CheckedChanged += onOptionChanged;
            Controls.Add(checkBox);
            return checkBox;
        }
    }
}
