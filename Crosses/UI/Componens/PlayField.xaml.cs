using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crosses.UI.Componens
{
    /// <summary>
    /// Interaction logic for PlayField.xaml
    /// </summary>
    public partial class PlayField : UserControl
    {
        private readonly Size _size;
        private readonly BitmapImage _crossImage;
        private readonly BitmapImage _cross2Image;
        private readonly BitmapImage _zeroImage;

        public PlayField(Size size)
        {
            InitializeComponent();

            var crossImageUri = new Uri("pack://application:,,/UI/Images/cross.png");
            _crossImage = new BitmapImage(crossImageUri);

            var cross2ImageUri = new Uri("pack://application:,,/UI/Images/cross2.png");
            _cross2Image = new BitmapImage(cross2ImageUri);

            var zeroImageUri = new Uri("pack://application:,,/UI/Images/zero.png");
            _zeroImage = new BitmapImage(zeroImageUri);

            _size = size;
            field.Dispatcher.InvokeAsync(() =>
            {
                for (int i = 0; i < size.Height; i++)
                {
                    field.ColumnDefinitions.Add(new ColumnDefinition());
                }

                for (int j = 0; j < size.Width; j++)
                {
                    field.RowDefinitions.Add(new RowDefinition());
                }

                for (int i = 0; i < size.Height; i++)
                {
                    for (int j = 0; j < size.Width; j++)
                    {
                        var button = new Button();
                        button.SetValue(Grid.ColumnProperty, i);
                        button.SetValue(Grid.RowProperty, j);
                        button.Content = new Image()
                        {
                            Source = _cross2Image
                        };
                        button.Click += OnButtonClick;
                        button.Name = $"Button_{i}_{j}";
                        field.Children.Add(button);
                    }
                }
            });
        }

        private void OnButtonClick(object sender, RoutedEventArgs args)
        {

        }
    }
}
