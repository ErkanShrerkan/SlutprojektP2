using System;
using System.Collections.Generic;

namespace Komplettering
{
    class Program
    {
        static Random gen = new Random();
        static Dictionary<string, DoThing> actions = new Dictionary<string, DoThing>();
        static int amount;
        static int sides;
        static int value;
        static int total;
        static int x;
        static int y;

        static void Main(string[] args)
        {
            Setup();
            while (true)
            {
                Console.WriteLine("1) Throw dice");
                Console.WriteLine("2) Get random cards");
                Console.WriteLine("3) Get a number between x and y");
                Console.WriteLine();

                try
                {
                    actions[Console.ReadLine()]();
                    Console.WriteLine();
                }
                catch (Exception)
                {
                    Console.WriteLine("\nERROR::INVALID INPUT\n");
                }
            }
        }

        static void Setup()
        {
            actions["1"] = ThrowDice;
            actions["2"] = PickACard;
            actions["3"] = RandomNumber;
        }

        delegate void DoThing();

        static void ThrowDice()
        {
            Console.WriteLine("\nHow many dice would you like to throw?");
            amount = TryParse();
            Console.WriteLine("\nHow many sides does the dice have?");
            sides = TryParse();

            for (int i = 0; i < amount; i++)
            {
                value = gen.Next(sides) + 1;
                total += value;
                Console.WriteLine("die #{0} rolled {1}", i+1, value);
            }
            Console.WriteLine("\nTotal: {0}\n" , total);
            total = 0;
        }

        static void PickACard()
        {
            var cards = new List<Card>();

            Console.WriteLine("\nHow many cards would you like?");
            amount = TryParse();

            for (int i = 0; i < amount; i++) 
            {
                var Card = new Card();

                Card = Card.CardGenerator();

                cards.Add(Card);
            }
            Console.WriteLine();
            for (int i = 0; i < cards.Count; i++)
            {
                int hash = i + 1;
                Console.WriteLine("Card #{0} is " + cards[i].name + " of " + cards[i].suit, i+1);

                value = Value(cards[i].value, total);

                total += value;
            }
            Console.WriteLine("\nTotal: {0}", total);
            total = 0;
        }

        static void RandomNumber()
        {
            Console.WriteLine("\nx = ?");
            x = TryParse();
            Console.WriteLine("y = ?");
            y = TryParse();

            Console.WriteLine("\nYour random number is: {0}", gen.Next(x, y + 1));
        }

        static int TryParse()
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int number))
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("You have to enter a number");
                }
            }
        }

        static int Value(int value, int total)
        {
            if (total < 21 && value == 1)
            {
                value = 11;
            }

            return value;
        }
    }

    class Card // Gammal klass så döm inte strukturen
    {
        Random gen = new Random();
        public int value;
        public string suit;
        public int suitValue;
        public string name;
        public int nameValue;

        public Card CardGenerator()
        {
            int x = Randomizer(1, 13);
            int y = Randomizer(1, 4);
            int z = x;
            string s = Suit(y);
            string n = Name(x);

            if (x > 10)
            {
                z = 10;
            }

            var Card = new Card() { name = n, suit = s, nameValue = x, suitValue = y, value = z };

            return Card;
        }

        static string Name(int x)
        {
            string n = "";
            if (x > 10 || x == 1)
            {
                if (x == 1)
                {
                    n = "Ace";
                }
                if (x == 11)
                {
                    n = "Jack";
                }
                if (x == 12)
                {
                    n = "Queen";
                }
                if (x == 13)
                {
                    n = "King";
                }
            }
            else
            {
                n = x + "";
            }

            return n;
        }

        public int Randomizer(int x, int y)
        {
            int i = gen.Next(x, y);

            return i;
        }

        public string Suit(int y)
        {
            string s = "";

            if (y == 1)
            {
                s = "Hearts";
            }
            if (y == 2)
            {
                s = "Clubs";
            }
            if (y == 3)
            {
                s = "Diamonds";
            }
            if (y == 4)
            {
                s = "Spades";
            }

            return s;
        }
    }
}

