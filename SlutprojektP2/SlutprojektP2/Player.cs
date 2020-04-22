using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutprojektP2
{
    class Player : Character
    {
        public Player()
        {
            collidableTiles = new char[] { '#', '¤', '&' };
            directions = new bool[] { up = true, down = true, left = true, right = true };
            name = "Player";
            Level = 1;
            xpToLevelUp = 10 + (5 * Level);
            currentXp = 0;
            MaxHP = 10 + (3 * Level);
            hp = MaxHP;
            DodgeChance = 20;
            Armor = 2;
        }

        public void PlayerController(char[,] tiles)
        {
            var key = Console.ReadKey(true); // väntar på input

            if (Game.isPaused) // om spelet är pausat kan inte spelaren flytta sin avatar
            {
                // här skulle jag kunna sätta in pausmeny inputs men eftersom att jag inte har en pausmeny är den tom
            }
            else
            {
                directions = CheckValidDirections(tiles); // kollar möjligheten för rörelse i alla riktningar, om en är false gills inte den 

                if (key.Key == ConsoleKey.RightArrow && directions[3]) // höger
                {
                    Move(1, 0, tiles);
                }
                if (key.Key == ConsoleKey.LeftArrow && directions[2]) // vänster
                {
                    Move(-1, 0, tiles);
                }
                if (key.Key == ConsoleKey.UpArrow && directions[0]) // upp
                {
                    Move(-1, 1, tiles);
                }
                if (key.Key == ConsoleKey.DownArrow && directions[1]) // ner
                {
                    Move(1, 1, tiles);
                }
            }

            if (key.Key == ConsoleKey.Escape) // klickar man på escape utanför ett battle stängs spelet av
            {
                Environment.Exit(0);
            }
            if (key.Key == ConsoleKey.Tab) // tab är pausknappen
            {
                if (Game.isPaused)
                {
                    Game.isPaused = false;
                }
                else if (!Game.isPaused)
                {
                    Game.isPaused = true;
                }
            }
        }

        void Move(int direction, int axis, char[,] tiles)
        {
            Game.ColorMap(tiles, Pos[0], Pos[1], "update"); // ritar den nuvarande koordinaten för spelaren svart
            Pos[axis] += direction; // ändrar spelarens koordinat för att färgläggas senare
            Game.canRollForEncounter = true; // eftersom att spelaren flyttade sig är denne mottaglig för eb encounter
        }

        bool[] CheckValidDirections(char[,] tiles)
        {
            // listan med tiles som spelaren kan kollidera med jämförs med de (4) närliggande rutorna
            // om en närliggande ruta innehåller en tile som kolliderar med spelaren är denna riktning false och därmed ogiltig att röra sig till

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

            return directions;
        }
    }
}
