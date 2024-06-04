using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okobere
{
    public enum Value
    {
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public class Card
    {
        public Card(Suit suit, Value value)
        {
            Suit = suit;
            Value = value;
        }

        public Suit Suit { get; private set; }
        public Value Value { get; private set; }

        public int GetIntValue()
        {
            return Value switch
            {
                Value.Seven => 7,
                Value.Eight => 8,
                Value.Nine => 9,
                Value.Ten => 10,
                Value.Jack => 2,
                Value.Queen => 2,
                Value.King => 2,
                Value.Ace => 11
            };
        }

        public override string ToString()
        {
            return Value switch
            {
                Value.Seven => "7",
                Value.Eight => "8",
                Value.Nine => "9",
                Value.Ten => "10",
                Value.Jack => "Svršek",
                Value.Queen => "Spodek",
                Value.King => "Král",
                Value.Ace => "Eso"
            };
        }
    }
}
