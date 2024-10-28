using ERD_Builder.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ERD_Builder.View
{
    public partial class ProjectPage : Window
    {
        private const double ZoomFactor = 0.1;
        private const double MinZoom = 0.5;
        private const double MaxZoom = 3.0;

        private bool _isResizing = false;
        private Point _resizeStartPoint;
        private string _resizeDirection;

        private Point _panStartPoint;
        private bool _isPanning;

        public ProjectPage(string projectName)
        {
            InitializeComponent();

            ProjectPageViewModel projectPageViewModel = new ProjectPageViewModel
            {
                projectName = projectName
            };
            DataContext = projectPageViewModel;

            Loaded += ProjectPage_Loaded;

            // Register mouse wheel event for zooming
            ERDCanvas.MouseWheel += ERDCanvas_MouseWheel;

            // Register mouse events for panning
            ERDCanvas.MouseDown += ERDCanvas_MouseDown;
            ERDCanvas.MouseMove += ERDCanvas_MouseMove;
            ERDCanvas.MouseUp += ERDCanvas_MouseUp;
        }

        private void ProjectPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the initial canvas size to match the current window size
            ERDCanvas.Width = ActualWidth;
            ERDCanvas.Height = ActualHeight;

            // Alternatively, add padding if needed
            ERDCanvas.Width = ActualWidth - 50; // for padding or borders
            ERDCanvas.Height = ActualHeight - 50; // for padding or borders
        }

        //===========================Start ZOOM and PANNING============================
        private void ERDCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scaleChange = e.Delta > 0 ? ZoomFactor : -ZoomFactor;
            double newScaleX = CanvasScaleTransform.ScaleX + scaleChange;
            double newScaleY = CanvasScaleTransform.ScaleY + scaleChange;

            if (newScaleX >= MinZoom && newScaleX <= MaxZoom)
            {
                CanvasScaleTransform.ScaleX = newScaleX;
                CanvasScaleTransform.ScaleY = newScaleY;
            }
        }

        private void ERDCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed) // Use middle mouse button for panning
            {
                _isPanning = true;
                _panStartPoint = e.GetPosition(this); // Capture start point of the pan
                ERDCanvas.CaptureMouse();
            }
        }

        private void ERDCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                var currentPoint = e.GetPosition(this);
                var offset = currentPoint - _panStartPoint;

                // Apply panning offsets
                CanvasTranslateTransform.X += offset.X;
                CanvasTranslateTransform.Y += offset.Y;

                _panStartPoint = currentPoint; // Update the start point for the next movement
            }
        }

        private void ERDCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isPanning)
            {
                _isPanning = false;
                ERDCanvas.ReleaseMouseCapture();
            }
        }

        //===========================END ZOOM and PANNING============================

        //===========================START Canvas resize============================

        private void ResizeHandle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle handle)
            {
                _isResizing = true;
                _resizeStartPoint = e.GetPosition(this);
                _resizeDirection = handle.Name;
                handle.CaptureMouse();
            }
        }

        private void ResizeHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isResizing)
            {
                Point currentPoint = e.GetPosition(this);
                double offsetX = currentPoint.X - _resizeStartPoint.X;
                double offsetY = currentPoint.Y - _resizeStartPoint.Y;

                switch (_resizeDirection)
                {
                    case "LeftResizeHandle":
                        ERDCanvas.Width = Math.Max(50, ERDCanvas.Width - offsetX);
                        Canvas.SetLeft(ERDCanvas, Canvas.GetLeft(ERDCanvas) + offsetX);
                        break;
                    case "RightResizeHandle":
                        ERDCanvas.Width = Math.Max(50, ERDCanvas.Width + offsetX);
                        break;
                    case "TopResizeHandle":
                        ERDCanvas.Height = Math.Max(50, ERDCanvas.Height - offsetY);
                        Canvas.SetTop(ERDCanvas, Canvas.GetTop(ERDCanvas) + offsetY);
                        break;
                    case "BottomResizeHandle":
                        ERDCanvas.Height = Math.Max(50, ERDCanvas.Height + offsetY);
                        break;
                    case "BottomRightResizeHandle":
                        ERDCanvas.Width = Math.Max(50, ERDCanvas.Width + offsetX);
                        ERDCanvas.Height = Math.Max(50, ERDCanvas.Height + offsetY);
                        break;
                }

                _resizeStartPoint = currentPoint;
            }
        }

        private void ResizeHandle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isResizing)
            {
                _isResizing = false;
                _resizeDirection = null;
                if (sender is Rectangle handle)
                {
                    handle.ReleaseMouseCapture();
                }
            }
        }

        //===========================END Canvas resize============================

        private void AddTable_Click(object sender, RoutedEventArgs e)
        {
            Border table = new Border
            {
                Width = 100,
                Height = 50,
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Child = new TextBlock
                {
                    Text = "New Table",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };

            Canvas.SetLeft(table, 50);
            Canvas.SetTop(table, 50);

            ERDCanvas.Children.Add(table);
            EnableTableDragging(table);

            // Dynamically resize canvas if needed
            UpdateCanvasSize();
        }
        private void EnableTableDragging(UIElement table)
        {
            bool isDragging = false;
            Point startPoint = new Point();

            table.MouseLeftButtonDown += (s, e) =>
            {
                isDragging = true;
                startPoint = e.GetPosition(ERDCanvas);
                table.CaptureMouse();
            };

            table.MouseMove += (s, e) =>
            {
                if (isDragging)
                {
                    var currentPosition = e.GetPosition(ERDCanvas);
                    var offsetX = currentPosition.X - startPoint.X;
                    var offsetY = currentPosition.Y - startPoint.Y;

                    // Calculate the new position of the table
                    double newLeft = Canvas.GetLeft(table) + offsetX;
                    double newTop = Canvas.GetTop(table) + offsetY;

                    // Get the canvas bounds
                    double canvasWidth = ERDCanvas.ActualWidth;
                    double canvasHeight = ERDCanvas.ActualHeight;

                    // Get the table dimensions
                    double tableWidth = ((FrameworkElement)table).ActualWidth;
                    double tableHeight = ((FrameworkElement)table).ActualHeight;

                    // Apply boundary checks to keep the table within the canvas
                    newLeft = Math.Max(0, Math.Min(newLeft, canvasWidth - tableWidth));
                    newTop = Math.Max(0, Math.Min(newTop, canvasHeight - tableHeight));

                    // Set the constrained position
                    Canvas.SetLeft(table, newLeft);
                    Canvas.SetTop(table, newTop);

                    // Update start point for the next movement
                    startPoint = currentPosition;
                }
            };

            table.MouseLeftButtonUp += (s, e) =>
            {
                isDragging = false;
                table.ReleaseMouseCapture();
            };
        }


        private void UpdateCanvasSize()
        {
            double maxX = 0, maxY = 0;
            foreach (UIElement element in ERDCanvas.Children)
            {
                double right = Canvas.GetLeft(element) + ((FrameworkElement)element).Width;
                double bottom = Canvas.GetTop(element) + ((FrameworkElement)element).Height;
                if (right > maxX) maxX = right;
                if (bottom > maxY) maxY = bottom;
            }

            ERDCanvas.Width = Math.Max(ERDCanvas.Width, maxX + 100); // Add padding if needed
            ERDCanvas.Height = Math.Max(ERDCanvas.Height, maxY + 100);
        }
    }
}
