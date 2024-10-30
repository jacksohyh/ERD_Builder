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


        private void AddTable_Click(object sender, RoutedEventArgs e)
        {
            // Create the main table container as a Border
            Border table = new Border
            {
                Width = 200,
                Height = 120,
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1)
            };

            // Create a Grid to hold the header and columns within the table
            Grid tableGrid = new Grid();
            table.Child = tableGrid;

            // Define rows: header row and two data rows
            tableGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });  // Header
            tableGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });  // Data Row 1
            tableGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });  // Data Row 2

            // Define two columns for the data rows
            tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header
            Border header = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = Brushes.LightGray,
                Padding = new Thickness(5),
                Child = new TextBlock
                {
                    Text = "Table Name",
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            Grid.SetRow(header, 0);
            Grid.SetColumnSpan(header, 2);  // Span across both columns
            tableGrid.Children.Add(header);

            // Add first data row, first column
            Border cell1 = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Child = new TextBlock
                {
                    Text = "Column 1",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            Grid.SetRow(cell1, 1);
            Grid.SetColumn(cell1, 0);
            tableGrid.Children.Add(cell1);

            // Add first data row, second column
            Border cell2 = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Child = new TextBlock
                {
                    Text = "Column 2",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            Grid.SetRow(cell2, 1);
            Grid.SetColumn(cell2, 1);
            tableGrid.Children.Add(cell2);

            // Add second data row, first column
            Border cell3 = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Child = new TextBlock
                {
                    Text = "Column 3",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            Grid.SetRow(cell3, 2);
            Grid.SetColumn(cell3, 0);
            tableGrid.Children.Add(cell3);

            // Add second data row, second column
            Border cell4 = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Child = new TextBlock
                {
                    Text = "Column 4",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            Grid.SetRow(cell4, 2);
            Grid.SetColumn(cell4, 1);
            tableGrid.Children.Add(cell4);

            // Position the table on the Canvas (adjust as needed)
            Canvas.SetLeft(table, 50);
            Canvas.SetTop(table, 50);

            // Add the table to the Canvas
            ERDCanvas.Children.Add(table);
            EnableTableDragging(table);

            // Dynamically resize the canvas if needed
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
