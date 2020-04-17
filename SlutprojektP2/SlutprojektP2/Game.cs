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
        bool oddOrEven = false; // true = jämt antal game loops, inverteras första (0e) loopen och blir därför true på en jämn loop
        Player player; // inte Character player; för att bara player behöver en controller
        public static bool isPaused = false;
        public static bool isResumed = true;
        public static Queue<string> messages = new Queue<string>();
        Queue<int[]> playerPositions = new Queue<int[]>();
        List<Character> characters = new List<Character>();
        int[][] comparePositions = new int[2][];
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
            player.LastPos = player.Pos;
            player.CurrentPos = player.Pos;
            //EnqueuePos(player.LastPos, 0);
            //EnqueuePos(player.Pos, 1);
            //player.LastPos = player.Pos;
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
                    messages.Enqueue(string.Format("CurrentPos: {0}x, {1}y     ", player.CurrentPos[0], player.CurrentPos[1]));
                    messages.Enqueue(string.Format("LastPos:    {0}x, {1}y     ", player.LastPos[0], player.LastPos[1]));

                    Encounter();
                    DrawCharacters();
                    DrawDisplay();
                    //
                }

                //EnqueuePos(player.Pos, 0);
                //player.LastPos = comparePositions[0];
                ////messages.Enqueue("Last " + comparePositions[0][0] + ", " + comparePositions[0][1]);

                player.LastPos = player.CurrentPos;

                player.PlayerController(MapAsArray);

                player.CurrentPos = player.Pos;

                //EnqueuePos(player.Pos, 1);
                ////messages.Enqueue("Current " + comparePositions[1][0] + ", " + comparePositions[1][1]);

                if (player.HP <= 0)
                {
                    gameOver = true;
                }
            }
        }

        void EnqueuePos(int[] pos, int index)
        {
            comparePositions[index] = pos;
            //messages.Enqueue(comparePositions[index][0] + ", " + comparePositions[index][1]);

            //if (oddOrEven == true)
            //{
            //    playerPositions[0] = player.Pos;
            //}
            //else if (oddOrEven == false)
            //{
            //    playerPositions[1] = player.Pos;
            //}

            //playerPositions.Enqueue(pos);

            //if (playerPositions.Count > 2)
            //{
            //    playerPositions.Dequeue();
            //}

            //for (int i = 0; i < playerPositions.Count; i++)
            //{
            //    comparePositions[i] = playerPositions.Dequeue();
            //}

            //for (int i = 0; i < comparePositions.Length; i++)
            //{
            //    playerPositions.Enqueue(comparePositions[i]);
            //}

            //foreach (var pos in playerPositions)
            //{
            //    oddOrEven = !oddOrEven;

            //    if (oddOrEven)
            //    {
            //        comparePositions[0] = pos;
            //    }
            //    else
            //    {
            //        comparePositions[1] = pos;
            //    }
            //}
        }

        void Encounter()
        {
            if (player.CurrentPos != player.LastPos)
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
            else
            {
                messages.Enqueue("f");
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
