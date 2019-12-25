using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Crosses.UI.Componens;

namespace Crosses
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGameButtonHandler(object sender, RoutedEventArgs e)
        {
            FieldContainer.Dispatcher.Invoke(() =>
            {
                var size = new Size(int.Parse(HeightTextBox.Text), int.Parse(WidthTextBox.Text));
                FieldContainer.Children.Add(new PlayField(new Game(size)));
                StartButton.IsEnabled = false;
            });
        }

        private void HeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(HeightTextBox.Text, out var height) && height > 0)
            {
                if (int.TryParse(HeightTextBox.Text, out var width) && width > 0)
                {
                    StartButton.IsEnabled = true;
                }

                if (height <= 5 && WidthTextBox.Text == "")
                {
                    WidthTextBox.Text = height.ToString();
                }
            }
        }

        private void WidthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(WidthTextBox.Text, out var width) && width > 0)
            {
                if (int.TryParse(HeightTextBox.Text, out var height) && height > 0)
                {
                    StartButton.IsEnabled = true;
                }

                if (width <= 5 && HeightTextBox.Text == "")
                {
                    HeightTextBox.Text = width.ToString();
                }
            }
        }
    }
}
