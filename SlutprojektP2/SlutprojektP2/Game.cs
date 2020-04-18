using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

namespace SlutprojektP2
{
    class Game
    {
        int displayStartPosY;
        public static Random gen; // en random generator för hela spelet
        Map map;
        Player player; // inte Character player; för att bara player behöver en controller
        public static bool isPaused = false;
        public static bool isResumed = true;
        public static Queue<string> messages = new Queue<string>();
        Queue<int[]> playerPositions = new Queue<int[]>();
        List<Character> characters = new List<Character>();
        static List<Enemy> enemies;
        public bool gameOver = false;
        private char[,] fullMapArray;

        public Game()
        {
            gen = new Random();
            Start();
            Update();
        }

        void Start() // initianerar spelet och ställer in startvärden
        {
            map = new Map();
            player = new Player() { Pos = new int[2] { 8, 2 } }; // player start position
            Weapon.Sword(player);
            Console.CursorVisible = false;
            DrawMap();
            DrawDisplay();
            characters.Add(player);
        }

        void Update()
        {
            while (!gameOver)
            {
                enemies = new List<Enemy>() { new Archer(player), new Warrior(player) }; // varje game loop förnyas fienderna för att de ska levla ikapp spelaren samt fylla på hp
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

                    // spelet händer här

                    Encounter();
                    DrawCharacters();
                    DrawDisplay();
                    //
                }

                player.PlayerController(MapAsArray);

                if (player.HP <= 0)
                {
                    gameOver = true;
                }
            }
        }



        void Encounter()
        {
            if (gen.Next(100) < 7)
            {
                messages.Enqueue("You encountered an enemy!      ");
                DrawDisplay();
                Thread.Sleep(2000);
                Battle newBattle = new Battle(player, enemies[gen.Next(2)]);
                Resume();
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
            // visa stats och liknande
        }

        void DrawMap()
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < 32; y++) // kollar varje koordinat i kartan
            {
                for (int x = 0; x < 128; x++)
                {
                    //Console.SetCursorPosition(x, y);
                    ColorMap(MapAsArray, x, y, "init"); // färglägger tilen enligt dess markör
                }
            }
            Console.ResetColor();
        }

        void DrawDisplay() // skriver ut i meddelanderutan
        {
            displayStartPosY = 28 - messages.Count;

            if (messages.Count > 13)
            {
                for (int i = messages.Count; i > 12; i--)
                {
                    messages.Dequeue();
                }
            }

            foreach (var message in messages)
            {
                displayStartPosY++;
                Console.SetCursorPosition(6, displayStartPosY);
                Console.WriteLine(message);
            }
        }

        void DrawCharacters() // Ritar ut alla karaktärer, just nu finns bara en karaktär, spelaren
        {
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].Pos[0] = CheckValidPosition(characters[i].Pos[0], "x");
                characters[i].Pos[1] = CheckValidPosition(characters[i].Pos[1], "y");

                Console.SetCursorPosition(characters[i].Pos[0], characters[i].Pos[1]);

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

                Console.ResetColor();
            }
        }

        int CheckValidPosition(int i, string axis) // kollar om spelaren står på en giltig ruta. Är onödig eftersom att banan hindrar spelaren att nå dessa rutor
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

        char[,] MapAsArray // lägger kartans symboler i en tvådimensionell array
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

            switch (tiles[x, y]) // beroende på vilken symbol som ligger i arrayen målas en färg ut
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

        static void CaseIf(int x, int y, string state) // denna används för att skriva ut ett mellanslag på alla rutor utom den första 
        {
            if (x == 0 && y == 0 && state == "init")
            {

            }
            if (x != 0 || y != 0 && state == "init")
            {
                Console.Write(" ");
            }
        }
    }
}
