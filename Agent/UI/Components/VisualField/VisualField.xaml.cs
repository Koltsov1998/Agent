using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Agent.Graphs;
using Agent.Graphs.GraphCreators;
using Agent.Models;
using Agent.Solutions;
using SPoint = System.Windows.Point;
using Point = Agent.Others.Point;

namespace Agent.Components
{
    /// <summary>
    /// Interaction logic for VisualField.xaml
    /// </summary>
    public partial class VisualField : UserControl
    {

        public static readonly DependencyProperty ActionFieldProperty = DependencyProperty.Register("ActionField",
            typeof(ActionField), typeof(VisualField), new UIPropertyMetadata(
                OnContextChanged));

        public ActionField ActionField
        {
            set { SetValue(ActionFieldProperty, value); }
            get { return (ActionField) GetValue(ActionFieldProperty); }
        }

        private static DrawingVisualElement _drawingVisualElement;

        static VisualField()
        {
            var starImageUri = new Uri("pack://application:,,/UI/Images/star.png");
            _starImage = new BitmapImage(starImageUri);

            var cookieImageUri = new Uri("pack://application:,,/UI/Images/cookie.png");
            _cookieImage = new BitmapImage(cookieImageUri);

            var heroImageUri = new Uri("pack://application:,,/UI/Images/hero.png");
            _heroImage = new BitmapImage(heroImageUri);
        }

        public VisualField()
        {
            InitializeComponent();

            _drawingVisualElement = new DrawingVisualElement();

            StackPanel.Children.Add(_drawingVisualElement);
            _stackPanel = StackPanel;
        }

        public static void RenderActionField(ActionField field, DrawingContext drawingContext)
        {
            int fieldWidth = field.Width * (_nodeWidth + _bordersWidth) + _bordersWidth;
            int fieldHeight = field.Height * (_nodeWidth + _bordersWidth) + _bordersWidth;

            _stackPanel.Height = fieldHeight;
            _stackPanel.Width = fieldWidth;

            drawingContext.DrawRectangle(Brushes.Black, (Pen) null,
                new Rect(0, 0, fieldWidth, fieldHeight));

            for (int i = 0; i < field.Height; i++)
            for (int j = 0; j < field.Width; j++)
            {
                var backGroundRectangle = new Rect(_bordersWidth + i * (_bordersWidth + _nodeWidth),
                    _bordersWidth + j * (_bordersWidth + _nodeWidth), _nodeWidth, _nodeWidth);

                switch (field.FieldNodes[i, j])
                {
                    case NodeType.Gross:
                    {
                        drawingContext.DrawRectangle(Brushes.ForestGreen,
                            (Pen) null,
                            backGroundRectangle);
                    }
                        break;
                    case NodeType.Rock:
                    {
                        drawingContext.DrawRectangle(Brushes.DarkSlateGray,
                            (Pen) null,
                            backGroundRectangle
                        );
                    }
                        break;
                    case NodeType.Star:
                    {
                        drawingContext.DrawRectangle(Brushes.ForestGreen,
                            (Pen) null,
                            backGroundRectangle);
                        drawingContext.DrawImage(_starImage,
                            new Rect(_bordersWidth + i * (_bordersWidth + _nodeWidth) + (_nodeWidth - _objectSize) / 2,
                                _bordersWidth + j * (_bordersWidth + _nodeWidth) + (_nodeWidth - _objectSize) / 2,
                                _objectSize, _objectSize));
                        break;
                    }

                    case NodeType.Cookie:
                    {
                        DrawObject(_cookieImage, drawingContext, i, j);
                        break;
                    }

                    case NodeType.Agent:
                    {
                        DrawObject(_heroImage, drawingContext, i, j);
                        break;
                    }
                }
            }
        }

        private static void DrawObject(ImageSource objectImage, DrawingContext drawingContext, int i, int j)
        {
            drawingContext.DrawRectangle(Brushes.ForestGreen,
                (Pen) null,
                new Rect(_bordersWidth + i * (_bordersWidth + _nodeWidth),
                    _bordersWidth + j * (_bordersWidth + _nodeWidth), _nodeWidth, _nodeWidth));
            drawingContext.DrawImage(objectImage,
                new Rect(_bordersWidth + i * (_bordersWidth + _nodeWidth) + (_nodeWidth - _objectSize) / 2,
                    _bordersWidth + j * (_bordersWidth + _nodeWidth) + (_nodeWidth - _objectSize) / 2,
                    _objectSize, _objectSize));
        }

        private static void RenderGraphMesh(GraphNode graph, DrawingContext drawingContext)
        {
            IterateOnGraph(graph, (context, point, arg3) => DrawArrow(context, point, arg3, Brushes.Red));

            void IterateOnGraph(GraphNode g, Action<DrawingContext, Point, Point> callback)
            {
                foreach (var childNode in g.ChildNodes)
                {
                    callback(drawingContext, g.Point, childNode.Point);
                    IterateOnGraph(childNode, callback);
                }
            }
        }

        private static void RenderSolutionRoute(List<GraphNode> solutionRoute, DrawingContext drawindContext)
        {
            var previousNode = solutionRoute[0];
            for (int i = 1; i < solutionRoute.Count; i++)
            {
                DrawArrow(drawindContext, previousNode.Point, solutionRoute[i].Point, Brushes.Purple);
                previousNode = solutionRoute[i];
            }
        }

        private static void DrawArrow(DrawingContext context, Point point1, Point point2, SolidColorBrush color)
        {
            var pen = new Pen(color, 1 * _scale);
            var startPoint = ConvertPoint(point1);
            var endPoint = ConvertPoint(point2);
            context.DrawLine(pen, startPoint, endPoint);
        }

        private static SPoint ConvertPoint(Point point)
        {
            return new SPoint(
                scaleCoordinate(point.X),
                scaleCoordinate(point.Y)
            );

            double scaleCoordinate(int coordinate)
            {
                return (coordinate + 0.5) * (_nodeWidth + _bordersWidth) + _bordersWidth;
            }
        }

        private static void OnContextChanged(DependencyObject e, DependencyPropertyChangedEventArgs args)
        {
            var field = args.NewValue as ActionField;
            field.FieldNodesChangedEvent += OnActionFieldNodesChanged;
            RerenderAll(field);
        }

        private static StackPanel _stackPanel;

        private static ImageSource _starImage;

        private static ImageSource _cookieImage;

        private static ImageSource _heroImage;

        private static double _scale { set; get; } = 1;

        private static int _bordersWidth => (int) (2 * _scale);
        private static int _nodeWidth => (int) (18 * _scale);
        private static int _objectSize => (int) (14 * _scale);

        private VisualFieldViewModel _viewModel => DataContext as VisualFieldViewModel;

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _scale = e.NewValue / 5 + 1;
            RerenderAll(ActionField);
        }

        private static void RerenderAll(ActionField af)
        {
            _drawingVisualElement.Dispatcher.Invoke(() =>
            {
                var drawingContext = _drawingVisualElement.drawingVisual.RenderOpen();
                RenderActionField(af, drawingContext);
                RenderGraphMesh(CreateGraph(af), drawingContext);
                RenderSolutionRoute(CreateSolutionRoute(af), drawingContext);
                drawingContext.Close();
            });
        }

        private static GraphNode CreateGraph(ActionField af)
        {
            //var graphCreator = new DfsGraphCreator();
            var graphCreator = new BfsGraphCreator();
            var graph = graphCreator.GenerateGraph(af);
            return graph;

        }

        private static List<GraphNode> CreateSolutionRoute(ActionField af)
        {
            BfsSolver solver = new BfsSolver();
            return solver.Solve(af);
        }

        private static void OnActionFieldNodesChanged(ActionField af)
        {
            RerenderAll(af);
        }
    }
}