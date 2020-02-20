using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutprojektP2
{
    class Player : Character
    {
        char[] collidableTiles;

        public Player()
        {
            collidableTiles = new char[] { '#', '¤', '&' };
            directions = new bool[] { up = true, down = true, left = true, right = true };
            name = "Player";
            hp = 100;
        }

        public void PlayerController(char[,] tiles)
        {
            var key = Console.ReadKey(true);

            directions = CheckValidDirections(tiles);

            if (key.Key == ConsoleKey.RightArrow && directions[3]) // höger
            {
                Walk(1, 0, tiles);
            }
            if (key.Key == ConsoleKey.LeftArrow && directions[2]) // vänster
            {
                Walk(-1, 0, tiles);
            }
            if (key.Key == ConsoleKey.UpArrow && directions[0]) // upp
            {
                Walk(-1, 1, tiles);
            }
            if (key.Key == ConsoleKey.DownArrow && directions[1]) // ner
            {
                Walk(1, 1, tiles);
            }
            if (key.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
        }

        void Walk(int direction, int axis, char[,] tiles)
        {
            Game.ColorMap(tiles, Pos[0], Pos[1], "update");
            Pos[axis] += direction;
        }

        bool[] CheckValidDirections(char[,] tiles)
        {
            for (int i = 0; i < collidableTiles.Length; i++)
            {
                if (collidableTiles.Contains(tiles[Pos[0] - 1, Pos[1]])) // vänster 
                {
                    directions[2] = false;
                }
                else
                {
                    directions[2] = true;
                }
                if (collidableTiles.Contains(tiles[Pos[0] + 1, Pos[1]])) // höger
                {
                    directions[3] = false;
                }
                else
                {
                    directions[3] = true;
                }
                if (collidableTiles.Contains(tiles[Pos[0], Pos[1] - 1])) // upp
                {
                    directions[0] = false;
                }
                else
                {
                    directions[0] = true;
                }
                if (collidableTiles.Contains(tiles[Pos[0], Pos[1] + 1])) // ner
                {
                    directions[1] = false;
                }
                else
                {
                    directions[1] = true;
                }
            }

            return directions;
        }
    }
}
