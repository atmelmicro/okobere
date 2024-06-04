using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace okobere
{

    enum GameState
    {
        Setup,
        WaitingForBet,
        WaitingForPlayer,
        Won,
        Lost,
        ToSetup
    }

    internal class Game
    {
        private Deck _player;
        private Deck _ai;
        private Deck _deck;
        private GameState _state;
        private int _bet;
        private int _money;
        private const int RISK = 4;

        public Game()
        {
            _player = new();
            _ai = new();
            _deck = Deck.Default(); 
            _state = GameState.Setup;
            _money = 100;
        }

        public void TakeCard(MainWindow window)
        {
            var card = _deck.Pop();
            _player.Add(card);
            DisplayCard(window, _player, 1, true);

            window.PlayerCardsValue.Content = $"Hodnota karet: {_player.Value}";

            if(_player.Value > 21)
            {
                _state = GameState.Lost;
                Next(window);
            }
        }

        public void DisplayCard(MainWindow window, Deck deck, int row, bool show = false)
        {
            var card = deck.Cards.Last();
            var node = new DisplayCard(card);
            if (show) node.Show();
            Grid.SetRow(node, row);
            Grid.SetColumn(node, deck.Cards.Count - 1);

            window.GameBoard.Children.Add(node);
        }

        public async void Next(MainWindow window)
        {
            switch(_state)
            {
                case GameState.Setup:
                    _player.Add(_deck.Pop());
                    DisplayCard(window, _player, 1, true);
                    _player.Add(_deck.Pop());
                    DisplayCard(window, _player, 1, true);

                    _ai.Add(_deck.Pop());
                    DisplayCard(window, _ai, 0);
                    _ai.Add(_deck.Pop());
                    DisplayCard(window, _ai, 0);

                    if(_player.Cards.First().Value == Value.Ace && _player.Cards.Last().Value == Value.Ace)
                    {
                        _state = GameState.Won;
                        Next(window);
                        return;
                    }
                    if (_ai.Cards.First().Value == Value.Ace && _ai.Cards.Last().Value == Value.Ace)
                    {
                        _state = GameState.Lost;
                        Next(window);
                        return;
                    }


                    window.StateLabel.Content = "Zadej sázku";
                    window.BetInput.Visibility = Visibility.Visible;
                    window.BetLabel.Visibility = Visibility.Visible;
                    window.PlayerCardsValue.Visibility = Visibility.Visible;
                    window.PlayerCardsValue.Content = $"Hodnota karet: {_player.Value}";

                    _state = GameState.WaitingForBet;
                    break;
                case GameState.WaitingForBet:
                    Console.WriteLine(window.BetInput.Text);

                    int val;
                    if (!int.TryParse(window.BetInput.Text, out val))
                    {
                        return;
                    }
                    if (val > _money || val < 1)
                    {
                        return;
                    }

                    _bet = val;
                    _money -= val;


                    window.StateLabel.Content = "Ber karty";
                    window.BetInput.Visibility = Visibility.Collapsed;
                    window.BetLabel.Content = $"Sázka: {_bet}";
                    window.PlayerCardsValue.Visibility = Visibility.Visible;
                    window.PlayerCardsValue.Content = $"Hodnota karet: {_player.Value}";
                    window.TakeCard.Visibility = Visibility.Visible;
                    window.Money.Content = $"Peníze: {_money}";

                    _state = GameState.WaitingForPlayer;
                    break;
                case GameState.WaitingForPlayer:
                    while (_ai.Value + RISK <= 21)
                    {
                        _ai.Add(_deck.Pop());
                        DisplayCard(window, _ai, 0);
                        await Task.Delay(500);
                    }

                    if (_ai.Value > 21)
                    {
                        _state = GameState.Won;
                    } else if(_player.Value > _ai.Value)
                    {
                        _state = GameState.Won;
                    } else
                    {
                        _state = GameState.Lost;
                    }

                    Next(window);
                    break;
                case GameState.Lost:
                    window.StateLabel.Content = "Prohrál jsi";
                    window.BetInput.Visibility = Visibility.Collapsed;
                    window.BetLabel.Visibility = Visibility.Collapsed;
                    window.PlayerCardsValue.Visibility = Visibility.Visible;
                    window.TakeCard.Visibility = Visibility.Collapsed;

                    foreach(var card in window.GameBoard.Children)
                    {
                        ((DisplayCard)card).Show();
                    }

                    _bet = 0;
                    window.Money.Content = $"Peníze: {_money}";

                    window.AICardsValue.Visibility = Visibility.Visible;
                    window.AICardsValue.Content = $"Hodnota karet AI: {_ai.Value}";

                    if(_money == 0)
                    {
                        var result = MessageBox.Show("nemáš peníze", "nauč se gamblit", MessageBoxButton.OK);
                        Environment.Exit(0);
                    }

                    _state = GameState.ToSetup;
                    break;
                case GameState.Won:
                    window.StateLabel.Content = "Vyhrál jsi";
                    window.BetInput.Visibility = Visibility.Collapsed;
                    window.BetLabel.Visibility = Visibility.Collapsed;
                    window.PlayerCardsValue.Visibility = Visibility.Visible;
                    window.TakeCard.Visibility = Visibility.Collapsed;

                    window.AICardsValue.Visibility = Visibility.Visible;
                    window.AICardsValue.Content = $"Hodnota karet AI: {_ai.Value}";

                    foreach (var card in window.GameBoard.Children)
                    {
                        ((DisplayCard)card).Show();
                    }

                    _money += _bet * 3;
                    _bet = 0;
                    window.Money.Content = $"Peníze: {_money}";

                    _state = GameState.ToSetup;
                    break;

                case GameState.ToSetup:
                    window.StateLabel.Content = "Hra je připravena";
                    window.BetInput.Visibility = Visibility.Collapsed;
                    window.BetLabel.Visibility = Visibility.Collapsed;
                    window.PlayerCardsValue.Visibility = Visibility.Collapsed;
                    window.TakeCard.Visibility = Visibility.Collapsed;
                    window.AICardsValue.Visibility = Visibility.Collapsed;

                    window.GameBoard.Children.Clear();
                    _player.Cards.Clear();
                    _ai.Cards.Clear();

                    window.BetLabel.Content = $"Sázka: {_bet}";

                    _state = GameState.Setup;
                    break;
            }
        }
    }
}
