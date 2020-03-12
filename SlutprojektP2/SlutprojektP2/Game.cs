using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutprojektP2
{
    class Game
    {
        Map map;
        Player player; // inte Character player; för att bara player behöver en controller
        public static bool isPaused = false;
        public static bool isResumed = true;
        Queue<Message> messages = new Queue<Message>();
        List<Character> characters = new List<Character>();
        bool gameOver = false;
        private char[,] fullMapArray;

        public Game()
        {
            Start();
            Update();
        }

        void Start()
        {
            map = new Map();
            Console.CursorVisible = false;
            DrawMap();
            DrawDisplay();
            player = new Player() { Pos = new int[2] { 8, 2 } }; // player start position
            characters.Add(player);
        }

        void Update()
        {
            while (!gameOver)
            {
                if (isPaused)
                {
                    if (isResumed)
                    {
                        Pause();
                        DrawPauseMenu();
                    }
                }
                else if (!isPaused)
                {
                    if (!isResumed)
                    {
                        Resume();
                    }
                    DrawCharacters();
                }
                player.PlayerController(MapAsArray);
            }
        }

        void Resume()
        {
            Console.Clear();
            DrawMap();
            DrawCharacters();
            isResumed = true;
            isPaused = false;
        }

        void Pause()
        {
            Console.Clear();
            isPaused = true;
            isResumed = false;
        }

        void DrawPauseMenu()
        {
            
        }

        void DrawMap()
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < 32; y++) // kollar varje koordinat i kartan
            {
                for (int x = 0; x < 128; x++)
                {
                    //Console.SetCursorPosition(x, y);
                    ColorMap(MapAsArray, x, y, "init"); // färjlägger tilen enligt dess markör
                }
            }
            Console.ResetColor();
        }

        void DrawDisplay()
        {
            Console.SetCursorPosition(6, 27);
            Console.WriteLine("Welcome to the Game!");
        }

        void DrawCharacters()
        {
            for (int i = 0; i < characters.Count; i++)
            {
                //Console.WriteLine(roundedPosX + " "+ roundedPosY+ "                          ");
                //Console.ReadLine();

                characters[i].Pos[0] = CheckValidPosition(characters[i].Pos[0], "x");
                characters[i].Pos[1] = CheckValidPosition(characters[i].Pos[1], "y");

                Console.SetCursorPosition(characters[i].Pos[0], characters[i].Pos[1]);

                //ColorMap(tiles, LastPos[0], LastPos[1], "update");

                if (characters[i].Name == "Player") // spelarens karaktär är röd
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(" ");
                }
                else // alla andra karaktärer är gula (finns inga ännu)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write(" ");
                }

                //Console.SetCursorPosition(0, 15);
                //Console.WriteLine("x = {0}; y = {1}               ", player.Pos[0], player.Pos[1]);

                Console.ResetColor();
            }
        }

        int CheckValidPosition(int i, string axis)
        {
            if (i < 0)
            {
                i = 0;
                return i;
            }
            if (axis == "x")
            {
                if (i >= map.width - 1)
                {
                    i = map.width - 1;
                    return i;
                }
                else return i;
            }
            if (axis == "y")
            {
                if (i >= map.height - 1)
                {
                    i = map.height - 1;
                    return i;
                }
                else return i;
            }
            else return 0;
        }

        char[,] MapAsArray
        {
            get
            {
                char[] mapArray = map.level.ToCharArray();

                fullMapArray = new char[map.width, map.height];

                for (int y = 0; y < map.height; y++)
                {
                    for (int x = 0; x < map.width; x++)
                    {
                        fullMapArray[x, y] = mapArray[(128 * y) + x];
                    }
                }

                return fullMapArray;
            }
        }

        public static void ColorMap(char[,] tiles, int x, int y, string state)
        {
            if (state == "update")
            {
                Console.SetCursorPosition(x, y);
            }

            switch (tiles[x, y])
            {
                case '.':
                    CaseIf(x, y, state);
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case '#':
                    CaseIf(x, y, state);
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case '¤':
                    CaseIf(x, y, state);
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;
                case '&':
                    CaseIf(x, y, state);
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case '+':
                    CaseIf(x, y, state);
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
            }
        }

        static void CaseIf(int x, int y, string state)
        {
            if (x == 0 && y == 0 && state == "init")
            {
                //Console.SetCursorPosition(0, 0);
            }
            if (x != 0 || y != 0 && state == "init")
            {
                Console.Write(" ");
            }
            if (state == "update")
            {

            }
        }
    }
}
