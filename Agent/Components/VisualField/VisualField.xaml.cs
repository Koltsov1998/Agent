using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Agent.Models;

namespace Agent.Components
{
    /// <summary>
    /// Interaction logic for VisualField.xaml
    /// </summary>
    public partial class VisualField : UserControl
    {
        public static readonly DependencyProperty ActionFieldProperty =
            DependencyProperty.Register("ActionField", typeof(ActionField), typeof(VisualField), new UIPropertyMetadata(
                OnActionFieldChanged));

        public ActionField ActionField
        {
            set { SetValue(ActionFieldProperty, value); }

            get { return (ActionField) GetValue(ActionFieldProperty); }
        }

        private static DrawingVisualElement _drawingVisualElement;

        static VisualField()
        {
            try
            {
                var starImageUri = new Uri("pack://application:,,/Images/star.png");
                _starImage = new BitmapImage(starImageUri);

                var cookieImageUri = new Uri("pack://application:,,/Images/cookie.png");
                _cookieImage = new BitmapImage(cookieImageUri);

                var heroImageUri = new Uri("pack://application:,,/Images/hero.png");
                _heroImage = new BitmapImage(heroImageUri);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public VisualField()
        {
            InitializeComponent();

            _drawingVisualElement = new DrawingVisualElement();

            StackPanel.Children.Add(_drawingVisualElement);
            _stackPanel = StackPanel;
        }

        public static void RenderActionField(ActionField field)
        {
            var drawingContext = _drawingVisualElement.drawingVisual.RenderOpen();

            int fieldWidth = field.Width * (_nodeWidth + _bordersWidth) + _bordersWidth;
            int fieldHeight = field.Height * (_nodeWidth + _bordersWidth) + _bordersWidth;

            _stackPanel.Height = fieldHeight;
            _stackPanel.Width = fieldWidth;

            drawingContext.DrawRectangle(Brushes.Black, (Pen) null,
                new Rect(0, 0, fieldWidth, fieldHeight));

            for (int i = 0; i < field.Height; i++)
            for (int j = 0; j < field.Width; j++)
            {
                switch (field.FieldNodes[i, j])
                {
                    case NodeType.Gross:
                    {
                        drawingContext.DrawRectangle(Brushes.ForestGreen,
                            (Pen) null,
                            new Rect(_bordersWidth + i * (_bordersWidth + _nodeWidth),
                                _bordersWidth + j * (_bordersWidth + _nodeWidth), _nodeWidth, _nodeWidth));
                    }
                        break;
                    case NodeType.Rock:
                    {
                        drawingContext.DrawRectangle(Brushes.DarkSlateGray,
                            (Pen) null,
                            new Rect(_bordersWidth + i * (_bordersWidth + _nodeWidth),
                                _bordersWidth + j * (_bordersWidth + _nodeWidth), _nodeWidth, _nodeWidth));
                    }
                        break;
                    case NodeType.Star:
                    {
                        drawingContext.DrawRectangle(Brushes.ForestGreen,
                            (Pen) null,
                            new Rect(_bordersWidth + i * (_bordersWidth + _nodeWidth),
                                _bordersWidth + j * (_bordersWidth + _nodeWidth), _nodeWidth, _nodeWidth));
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

            drawingContext.Close();
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

        private static void OnActionFieldChanged(DependencyObject e, DependencyPropertyChangedEventArgs args)
        {
            var field = args.NewValue as ActionField;
            RenderActionField(field);
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
            RenderActionField(ActionField);
        }
    }
}