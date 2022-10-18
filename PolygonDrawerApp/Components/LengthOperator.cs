using Microsoft.VisualBasic;
using PolygonDrawer;
using PolygonDrawerApp;
using Polygons.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonDrawerApp.Components
{
    /// <summary>
    /// Komponent na pasku narzędzi imożliwiający ustawienie długość krawędzi
    /// </summary>
    public class LengthOperator : Toolbar
    {
        private const int visiblePresicion = 2;

        private OptionButton button;
        private NumericUpDown numeric = new()
        {
            Width = 3 * FormConstants.MinimumControlSize,
            AutoSize = false,
            BorderStyle = BorderStyle.FixedSingle,
            DecimalPlaces = visiblePresicion,
            TextAlign = HorizontalAlignment.Center,
            Maximum = decimal.MaxValue,
            Minimum = 1,
        };


        public bool Lock
        {
            get => button.Lock;
            set 
            {
                button.Lock = value;
                numeric.Enabled = !value;
            }
        }

        public event Action<bool> OnLengthLockChanged;
        public event Action<float> OnLengthChanged;


        public decimal Value
        {
            get => numeric.Value;
            set => numeric.Value = value;
        }

        public LengthOperator()
        {
            Margin = Padding.Empty;
            AddDivider();
            numeric.Margin = new Padding(8, (32 - numeric.Height) / 2, 0, (32 - numeric.Height) / 2);
            button = AddTool(LockChanged, ConstraintSymbols.ConstLength, Resources.LengthConstraintText);
            button.Margin = Padding.Empty;
            Controls.Add(numeric);
            AddDivider();
            numeric.ValueChanged += Numeric_ValueChanged;
        }

        private void Numeric_ValueChanged(object? sender, EventArgs e)
        {
            OnLengthChanged?.Invoke((float)numeric.Value);
        }

        private void LockChanged(bool newValue)
        {
            numeric.Enabled = !newValue;
            OnLengthLockChanged?.Invoke(newValue);
        }
    }
}
