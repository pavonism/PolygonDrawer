using Polygons.Constraints;

namespace PolygonDrawer.Components
{
    /// <summary>
    /// Komponent na pasku narzędzi imożliwiający ustawienie długość krawędzi
    /// </summary>
    public class LengthOperator : Toolbar
    {
        private const decimal MinimumValue = 1;
        private const int VisiblePresicion = 2;
        private const int ControlSize = 3 * FormConstants.MinimumControlSize;
        private const int NumericPaddingLeftRight = 8;

        #region Fields and Properties
        private readonly CheckButton button;
        private readonly NumericUpDown numeric = new()
        {
            Width = ControlSize,
            AutoSize = false,
            BorderStyle = BorderStyle.FixedSingle,
            DecimalPlaces = VisiblePresicion,
            TextAlign = HorizontalAlignment.Center,
            Maximum = decimal.MaxValue,
            Minimum = MinimumValue,
        };

        public decimal Value
        {
            get => numeric.Value;
            set => numeric.Value = value;
        }

        public bool Lock
        {
            get => button.Lock;
            set 
            {
                button.Lock = value;
                numeric.Enabled = !value;
            }
        }
        #endregion

        #region Events
        public event Action<bool>? OnLengthLockChanged;
        public event Action<float>? OnLengthChanged;

        private void NumericValueChanged(object? sender, EventArgs e)
        {
            OnLengthChanged?.Invoke((float)numeric.Value);
        }

        private void LockChanged(bool newValue)
        {
            numeric.Enabled = !newValue;
            OnLengthLockChanged?.Invoke(newValue);
        }
        #endregion

        public LengthOperator()
        {
            Margin = Padding.Empty;
            numeric.Margin = new Padding(
                NumericPaddingLeftRight, (FormConstants.MinimumControlSize - numeric.Height) / 2,
                NumericPaddingLeftRight, (FormConstants.MinimumControlSize - numeric.Height) / 2);
            numeric.ValueChanged += NumericValueChanged;

            AddDivider();
            button = AddTool(LockChanged, ConstraintSymbols.ConstLength, Resources.LengthConstraintHint);
            button.Margin = Padding.Empty;
            Controls.Add(numeric);
            AddDivider();
        }
    }
}
