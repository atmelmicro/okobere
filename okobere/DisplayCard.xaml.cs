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

namespace okobere
{
    /// <summary>
    /// Interaction logic for DisplayCard.xaml
    /// </summary>
    public partial class DisplayCard : UserControl
    {
        private Card _card;
        public DisplayCard(Card card)
        {
            InitializeComponent();
            _card = card;
            Hide();
        }

        public void Show()
        {
            MainGrid.Background = new SolidColorBrush(Colors.White);
            var value = new Label();
            value.Content = _card.ToString();
            Grid.SetRow(value, 0);
            Grid.SetColumn(value, 0);
            Grid.SetColumnSpan(value, 2);
            value.FontSize = 18;

            var suit = new Label();
            suit.Content = _card.Suit switch {
                Suit.Spades => "♠",
                Suit.Hearts => "♥",
                Suit.Diamonds => "♦",
                Suit.Clubs => "♣"
            };
            suit.FontSize = 24;


            if (_card.Suit == Suit.Hearts || _card.Suit == Suit.Diamonds)
            {
                suit.Foreground = Brushes.Red;
                value.Foreground = Brushes.Red;
            }

            Grid.SetRow(suit, 1);
            Grid.SetColumn(suit, 1);

            MainGrid.Children.Add(value);
            MainGrid.Children.Add(suit);
        }

        public void Hide()
        {
            MainGrid.Children.Clear();
            MainGrid.Background = new SolidColorBrush(Colors.MediumVioletRed);
        }
    }
}
