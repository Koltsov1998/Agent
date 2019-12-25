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
        private readonly Game _game;
        private readonly BitmapImage _crossImage;
        private readonly BitmapImage _cross2Image;
        private readonly BitmapImage _zeroImage;

        public PlayField(Game game)
        {
            InitializeComponent();

            var crossImageUri = new Uri("pack://application:,,/UI/Images/cross.png");
            _crossImage = new BitmapImage(crossImageUri);

            var cross2ImageUri = new Uri("pack://application:,,/UI/Images/cross2.png");
            _cross2Image = new BitmapImage(cross2ImageUri);

            var zeroImageUri = new Uri("pack://application:,,/UI/Images/zero.png");
            _zeroImage = new BitmapImage(zeroImageUri);

            _size = game.FieldSize;
            _game = game;
            field.Dispatcher.InvokeAsync(() =>
            {
                for (int i = 0; i < _size.Height; i++)
                {
                    field.ColumnDefinitions.Add(new ColumnDefinition());
                }

                for (int j = 0; j < _size.Width; j++)
                {
                    field.RowDefinitions.Add(new RowDefinition());
                }

                for (int i = 0; i < _size.Height; i++)
                {
                    for (int j = 0; j < _size.Width; j++)
                    {
                        var button = new Button();
                        button.SetValue(Grid.ColumnProperty, i);
                        button.SetValue(Grid.RowProperty, j);
                        
                        button.Click += OnButtonClick;
                        button.Name = $"Button_{i}_{j}";
                        field.Children.Add(button);
                    }
                }
            });
        }

        private void OnButtonClick(object sender, RoutedEventArgs args)
        {
            var button = sender as Button;
            var coordinate = new Coordinate(int.Parse(button.Name.Split("_")[1]), int.Parse(button.Name.Split("_")[2]));
            _game.MakeTurn(coordinate);
            button.Content = new Image()
            {
                Source = _cross2Image
            };

            var enemyTurnCoordinate = _game.WaitForEnemyTurn();
            foreach (var b in field.Children)
            {
                var but = b as Button;
                if (but != null)
                {
                    if (but.Name == $"Button_{enemyTurnCoordinate.X}_{enemyTurnCoordinate.Y}")
                    {
                        but.Content = new Image()
                        {
                            Source = _zeroImage
                        };
                    }
                }
            }
        }
    }
}
