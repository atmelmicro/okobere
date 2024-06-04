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

namespace okobere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game _game;

        public MainWindow()
        {
            _game = new();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _game.Next(this);
        }

        private void TakeCard_Click(object sender, RoutedEventArgs e)
        {
            _game.TakeCard(this);
        }
    }
}