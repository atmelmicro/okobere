using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okobere
{
    internal class Deck
    {
        public List<Card> Cards { get; private set; }
        public int Value => Cards.Select(x => x.GetIntValue()).Sum();

        public Deck()
        {
            Cards = new();
        }

        static public Deck Default()
        {
            var deck = new Deck();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    deck.Cards.Add(new Card(suit, value));
                }
            }

            var rnd = new Random();
            deck.Cards = [.. deck.Cards.OrderBy(x => rnd.NextDouble())];

            return deck;
        }

        public Card Pop()
        {
            var card = Cards.Last();

            Cards.Remove(card);

            return card;
        }

        public void Add(Card card)
        {
            Cards.Add(card);
        }
    }
}
