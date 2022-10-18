using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonDrawer;

namespace PolygonDrawerApp.Components
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

            Controls.Add(label);
        }

        public void AddDivider()
        {
            Controls.Add(new Divider());
        }

        public OptionButton AddTool(Action<bool> handler, string glyph, string toolTip)
        {
            var tooltip = new ToolTip();

            var button = new OptionButton()
            {
                Width = FormConstants.MinimumControlSize,
                Height = FormConstants.MinimumControlSize,
                Margin = new Padding(2, 0, 2, 0),
                Text = glyph,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14, FontStyle.Bold),
            };

            button.FlatAppearance.BorderSize = 0;
            button.OnOptionChanged += handler;

            tooltip.SetToolTip(button, toolTip);
            Controls.Add(button);
            return button;
        }

        public CheckBox AddOption(string text, EventHandler onOptionChanged)
        {
            var checkBox = new CheckBox()
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = text,
            };


            checkBox.CheckedChanged += onOptionChanged;
            Controls.Add(checkBox);
            return checkBox;
        }
    }
}
