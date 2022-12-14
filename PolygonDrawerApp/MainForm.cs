using PolygonDrawer.Components;
using PolygonDrawer.Demo;
using PolygonDrawer.ShapeVisitors;
using Polygons;
using Polygons.Constraints;
using Polygons.Shapes;
using ShapeSketcher;

namespace PolygonDrawer
{
    public partial class MainForm : Form
    {
        private TableLayoutPanel mainTableLayout = new();
        private Toolbar toolbar = new();
        private LengthOperator lengthOperator = new();

        private ShapeSketcher<IPolygonShape> sketcher;
        private PolygonManager polygonManager = new();
        private SelectionVisitor selectionVisitor;

        public MainForm()
        {
            InitializeToolbar();
            InitializeSketcher();
            ArrangeComponents();
            InitializeForm();

            LoadDemo();
        }

        private void InitializeForm()
        {
            this.Text = Resources.ProgramTitle;
            this.MinimumSize = new Size(FormConstants.MinimumWindowSizeX, FormConstants.MinimumWindowSizeY);
            this.Size = new Size(FormConstants.InitialWindowSizeX, FormConstants.InitialWindowSizeY);
        }

        private void ArrangeComponents()
        {
            this.mainTableLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.mainTableLayout.RowCount = FormConstants.MainFormRowCount;
            this.mainTableLayout.RowStyles.Add(new RowStyle() { Height = FormConstants.MinimumControlSize });
            this.mainTableLayout.RowStyles.Add(new RowStyle() { SizeType = SizeType.AutoSize });

            this.mainTableLayout.Controls.Add(this.sketcher, 0, 1);
            this.mainTableLayout.Controls.Add(this.toolbar, 0, 0);
            this.mainTableLayout.Dock = DockStyle.Fill;
            this.Controls.Add(mainTableLayout);
        }

        private void InitializeToolbar()
        {
            this.toolbar.AddLabel(Resources.ProgramTitle);
            this.toolbar.AddDivider();
            this.toolbar.AddButton(LoadDemoHandler, Resources.ReloadDemoGlyph, Resources.ReloadDemoHint);
            this.toolbar.AddButton(ClearHandler, Resources.ClearGlyph, Resources.ClearTextHint);
            this.toolbar.AddTool(PerpendicularHandler, ConstraintSymbols.Perpendicular, Resources.PerpendicularModeHint);
            this.toolbar.AddDivider();
            this.toolbar.AddOption(Resources.BresenhamOptionText, BresenhamOptionChangedHandler, Resources.BresenhamTooltip);

            this.lengthOperator.Hide();
            this.lengthOperator.OnLengthLockChanged += LengthLockChangedHandler;
            this.lengthOperator.OnLengthChanged += LengthChangedHandler;
            this.toolbar.Controls.Add(this.lengthOperator);

            this.selectionVisitor = new SelectionVisitor(lengthOperator);
        }

        private void InitializeSketcher()
        {
            sketcher = new ShapeSketcher<IPolygonShape>(new PolygonComposer(), this.polygonManager);
            sketcher.Dock = DockStyle.Fill;
            sketcher.OnSelectionChanged += ShapeSelectionChangedHandler;
        }

        private void LoadDemo()
        {
            sketcher.Clear();
            var DemoScene = new DemoScene();
            DemoScene.Load(polygonManager);
            sketcher.Refresh();
        }

        #region Event Handlers
        private void BresenhamOptionChangedHandler(object? sender, EventArgs e)
        {
            sketcher.RenderMode = sketcher.RenderMode == RenderMode.Default ? RenderMode.Bresenham : RenderMode.Default; 
        }

        private void LengthChangedHandler(float newValue)
        {
            var lengthSetter = new LengthSetter(newValue);
            sketcher.Selection?.Visit(lengthSetter);
            sketcher.Refresh();
        }

        private void LengthLockChangedHandler(bool newValue)
        {
            var constraintGiver = new ConstLengthSetter(!newValue ? null : (float)this.lengthOperator.Value);
            sketcher.Selection?.Visit(constraintGiver);
            sketcher.Refresh();
        }

        private void ShapeSelectionChangedHandler(IPolygonShape selection)
        {
            selection.Visit(selectionVisitor);
        }

        private void PerpendicularHandler(bool newValue)
        {
            polygonManager.ManagerMode = newValue ? ManagerMode.AddRelation : ManagerMode.Select;
        }

        private void LoadDemoHandler(object? sender, EventArgs e)
        {
            LoadDemo();
        }

        private void ClearHandler(object? sender, EventArgs e)
        {
            sketcher.Clear();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            sketcher.CanvasSize = this.Size;
        }
        #endregion
    }
}