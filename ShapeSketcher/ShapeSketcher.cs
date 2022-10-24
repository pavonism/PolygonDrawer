
namespace ShapeSketcher
{
    public class ShapeSketcher<ShapeType> : PictureBox where ShapeType : IRenderableShape
    {
        #region Fields and Properties
        private Bitmap drawingContext;
        private IShapeComposer<ShapeType> shapeComposer;
        private IShapeManager<ShapeType> shapeManager;
        private bool isDragged;
        private PointF lastClickPoint;

        public IShapeComposer<ShapeType> ShapeComposer
        {
            get => shapeComposer;
            set
            {
                if(shapeComposer != null)
                {
                    shapeComposer.OnShapeCreation -= ShapeCreationHandler;
                    shapeComposer.OnShapeCancel -= ShapeCancelHandler;
                }

                shapeComposer = value ?? throw new ArgumentNullException();
                shapeComposer.OnShapeCreation += ShapeCreationHandler;
                shapeComposer.OnShapeCancel += ShapeCancelHandler;
            }
        }

        public IShapeManager<ShapeType> ShapeManager
        {
            get => shapeManager;
            set
            {
                if(shapeManager != null)
                    shapeManager.OnSelectionChanged -= SelectionChangedHandler;

                shapeManager = value ?? throw new ArgumentNullException();
                shapeManager.OnSelectionChanged += SelectionChangedHandler;
            }
        }

        public ControlMode ControlMode { get; set; } = ControlMode.Select;

        public Size CanvasSize
        {
            get => drawingContext.Size;
            set
            {
                drawingContext.Dispose();
                drawingContext = new Bitmap(value.Width, value.Height);
                this.Image = drawingContext;
                Refresh();
            }
        }

        public RenderMode RenderMode { get; set; } = RenderMode.Default;
        public ShapeType? Selection => ShapeManager.Selection;
        #endregion

        #region Events
        public event Action<ShapeType>? OnSelectionChanged;
        #endregion

        public ShapeSketcher(IShapeComposer<ShapeType> shapeComposer, IShapeManager<ShapeType> shapeManager)
        {
            drawingContext = new Bitmap(ControlConstants.CanvasDefaultWidth, ControlConstants.CanvasDefaultHeight);
            this.Image = drawingContext;

            this.ShapeComposer = shapeComposer;
            this.ShapeManager = shapeManager;

            this.MouseDown += MouseDownHandler;
            this.MouseMove += MouseMoveHandler;
            this.MouseUp += MouseUpHandler;
        }

        #region Event Handlers
        private void MouseUpHandler(object? sender, MouseEventArgs e)
        {
            PointF mousePoint = new(e.X, e.Y);

            if (mousePoint.X == lastClickPoint.X && mousePoint.Y == lastClickPoint.Y)
            {
                switch (ControlMode)
                {
                    case ControlMode.Select:
                        ShapeManager.MouseClick(mousePoint, e.Button);
                        break;
                    case ControlMode.Drawing:
                        ShapeComposer.MouseClick(mousePoint, e.Button);
                        break;
                }
            }

            isDragged = false;
        }

        private void MouseMoveHandler(object? sender, MouseEventArgs e)
        {
            PointF mousePoint = new(e.X, e.Y);

            switch (ControlMode)
            {
                case ControlMode.Select:
                    if (isDragged)
                        ShapeManager.MouseDrag(lastClickPoint, mousePoint, e.Button);
                    else
                        ShapeManager.MouseMove(mousePoint);
                    break;
                case ControlMode.Drawing:
                    if (isDragged)
                        ShapeComposer.MouseDrag(lastClickPoint, mousePoint, e.Button);
                    else
                        ShapeComposer.MouseMove(mousePoint);
                    break;
            }

            Refresh();
        }

        private void MouseDownHandler(object? sender, MouseEventArgs e)
        {
            PointF clickPoint = new(e.X, e.Y);

            if(lastClickPoint == clickPoint)
            {
                switch (ControlMode)
                {
                    case ControlMode.Select:
                        ShapeManager.MouseDoubleClick(clickPoint, e.Button);
                        break;
                    case ControlMode.Drawing:
                        ShapeComposer.MouseDoubleClick(clickPoint, e.Button);
                        break;
                }

                lastClickPoint = PointF.Empty;
            }

            isDragged = true;
            lastClickPoint = clickPoint;

            switch (ControlMode)
            {
                case ControlMode.Select:
                    ShapeManager.MouseDown(clickPoint, e.Button);
                    if (ShapeManager.Selection == null)
                    {
                        ShapeComposer.MouseDown(clickPoint, e.Button);
                        ControlMode = ControlMode.Drawing;
                    }
                    break;
                case ControlMode.Drawing:
                    ShapeComposer.MouseDown(clickPoint, e.Button);
                    break;
            }

        }

        private void ShapeCreationHandler(ShapeType newShape)
        {
            if(newShape != null)
                this.ShapeManager.AddShape(newShape);
            this.ControlMode = ControlMode.Select;
        }

        private void ShapeCancelHandler()
        {
            this.ControlMode = ControlMode.Select;
        }

        private void SelectionChangedHandler(ShapeType selection)
        {
            this.OnSelectionChanged?.Invoke(selection);
        }
        #endregion

        #region Rendering
        public override void Refresh()
        {
            using (var g = Graphics.FromImage(drawingContext))
            {
                g.Clear(Color.White);
            }

            ShapeManager.Refresh(drawingContext, RenderMode);
            ShapeComposer.Refresh(drawingContext, RenderMode);

            base.Refresh();
        }

        public void Clear()
        {
            shapeManager.Clear();

            using (var g = Graphics.FromImage(drawingContext))
            {
                g.Clear(Color.White);
            }

            Refresh();
        }

        #endregion
    }

}
