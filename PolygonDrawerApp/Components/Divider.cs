using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonDrawer;

namespace PolygonDrawerApp.Components
{

    /// <summary>
    /// Implementuje obiekt rozdzielający opcje dostępne na pasku z nadzędziami
    /// </summary>
    internal class Divider : Label
    {
        private const int DividerWidth = 2;
        private const int HorizontalPadding = 8;

        public Divider()
        {
            Text = string.Empty;
            BorderStyle = BorderStyle.Fixed3D;
            AutoSize = false;
            Width = DividerWidth;
            Height = FormConstants.MinimumControlSize;
            Margin = new Padding(HorizontalPadding, 0, HorizontalPadding, 0);
        }
    }
}
