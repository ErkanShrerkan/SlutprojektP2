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
        int displayStartPosY; // används för att flytta cursorn 
        public static Random gen; // en random generator för hela spelet
        Map map;
        public static Player player;
        public static bool isPaused = false; // används för att hålla koll på vilken state spelet är i
        public static bool isResumed = true; // ^
        public static bool canRollForEncounter = false; // bestämmer om spelaren kan slumpa ett battle
        public static Queue<string> messages = new Queue<string>(); // queue för att lätt ta bort det äldsta meddelandet
        List<Character> characters = new List<Character>(); // list för att det inte är förbestämt hur många characters det ska finnas
        static List<Enemy> enemies; // just nu skulle denna lika gärna vara en array eftersom att den inte förändras i längd
        public bool gameOver = false; 

        public Game()
        {
            gen = new Random();
            Start(); // initierar spelet
            Update(); // startar game loopen
        }

        void Start() // initianerar spelet och ställer in startvärden
        {
            // här skulle jag också vilja sätta konsollens storlek av buffer, bredd, typsnitt med mera
            map = new Map();
            player = new Player() { Pos = new int[2] { 8, 2 } }; // player start position
            Weapon.Sword(player); // armerar spelaren med ett svärd
            Console.CursorVisible = false; // gör cursorn osynlig så att man inte ser den flyttas runt hela tiden
            DrawMap(); // ritar kartan
            DrawDisplay(); // skriver ut alla meddelanden i queuen
            characters.Add(player); // sätter spelaren i characterlistan för senare rendering
        }

        void Update()
        {
            while (!gameOver) // kör så länge spelets krav för game over inte stämmer
            {
                enemies = new List<Enemy>() { new Archer(player), new Warrior(player) }; // varje game loop förnyas fienderna för att de ska levla ikapp spelaren samt fylla på hp
                if (isPaused) // om pausknappen trycks...
                {
                    if (isResumed) // ...och spelet inte redan är pausat...
                    {
                        Pause(); // ...så pausas spelet
                    }
                }
                else if (!isPaused) // om pausknappen trycks...
                {
                    if (!isResumed) // ...och spelet redan är pausat...
                    { 
                        Resume(); // ...så återgår spelet
                    }

                    //
                    DrawCharacters(); // ritar ut alla karaktärer på sina koordinater
                    DrawDisplay(); // displayen i det här spelet är meddelanderutan och dess innehåll
                    //
                }

                player.PlayerController(MapAsArray); // tar en input varje loop

                // för debugging och demonstration av meddelandesystemet
                if (canRollForEncounter)
                {
                    messages.Enqueue("true                   "); // för true
                }
                else
                { 
                    messages.Enqueue("false                  ");
                }
                //

                if (canRollForEncounter) // om spelaren rört sig...
                {
                    Encounter(); // ...slumpar det om spelaren ska slåss mot en fiende
                    canRollForEncounter = false;
                }

                if (player.HP <= 0) // om spelarens hp är <=0 är spelet över
                {
                    gameOver = true;
                }
            }
        }

        void Encounter()
        {
            if (gen.Next(100) < 7) // 7% chans att möta en fiende varje loop
            {
                messages.Enqueue("You encountered an enemy!      "); // lägger till ett meddelande i queuen
                DrawDisplay(); // skriver utt det meddelandet
                Thread.Sleep(2000); // ger spelaren tid att läsa meddelandet
                Battle newBattle = new Battle(player, enemies[gen.Next(enemies.Count)]); // ett battle startas med spelaren och en slumpad fiende
                Resume(); // efter varje battle återgår spelet till att visa kartan med meddelandena
            }
        }

        void Resume()
        {
            Console.Clear(); // raderar allt på skärmen
            DrawMap(); // ritar kartan
            DrawCharacters(); // ritar karaktärerna
            isResumed = true; // spelet återgår
            isPaused = false; // ^
        }

        void Pause()
        {
            Console.Clear(); // raderar allt på skärmen
            DrawPauseMenu(); // just nu är denna tom men skulle kunna skriva ut en meny
            isPaused = true; // pausar spelet
            isResumed = false; // ^
        }

        void DrawPauseMenu()
        {
            // visa stats och liknande
        }

        void DrawMap()
        {
            Console.SetCursorPosition(0, 0); // sätter cursorn i övre vänstra hörnet
            for (int y = 0; y < 32; y++) // kollar varje y-koordinat i kartan
            {
                for (int x = 0; x < 128; x++) // kollar varje x-koordinat i kartan
                {
                    ColorMap(MapAsArray, x, y, "init"); // färglägger tilen enligt dess markör
                }
            }
            Console.ResetColor();
        }

        void DrawDisplay() // skriver ut i meddelanderutan
        {
            if (messages.Count > 14) // om fler än 14 meddelanden queueats...
            {
                // (negativ for loop för att den räknar ner till 13 från en större siffra)
                for (int i = messages.Count; i > 14; i--) // ...ska meddelanden dequeueas tills det inte finns fler än 13 kvar
                {
                    messages.Dequeue();
                }
            }

            displayStartPosY = 28 - messages.Count;

            foreach (var message in messages) // varje meddelande i queuen
            {
                displayStartPosY++; // ett större värde på y gör att koordinaten hamnar längre ner på skärmen
                Console.SetCursorPosition(6, displayStartPosY); // alla meddelanden skrivs därför uppifrån och ner
                Console.WriteLine(message);
            }
        }

        void DrawCharacters() // Ritar ut alla karaktärer, just nu finns bara en karaktär, spelaren
        {
            for (int i = 0; i < characters.Count; i++) // ifall fler spelare implementeras fungerar denna metod fortfarande
            {
                // en ogiltig koordinat kraschar spelet
                //characters[i].Pos[0] = CheckValidPosition(characters[i].Pos[0], "x"); // kollar om spelarens x koordinat är inom buffern
                //characters[i].Pos[1] = CheckValidPosition(characters[i].Pos[1], "y"); // kollar om spelarens y koordinat är inom buffern

                Console.SetCursorPosition(characters[i].Pos[0], characters[i].Pos[1]); // sätter cursorn på spelarens koordinat

                if (characters[i].Name == "Player") // spelarens karaktär är röd
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(" ");
                }
                else // alla andra karaktärer är gula, fiender till exempel (slumpas just nu men detta är i förebyggande syfte)
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

        char[,] MapAsArray // är en property för att det kändes lättare att förstå då
        {
            // om map.level ändras i framtiden (ie spelaren klarar av den nuvarande) behövs ingen kod ändras i denna property
            get
            {
                char[] mapArray = map.level.ToCharArray();  // lägger kartans symboler i en array

                char[,] fullMapArray = new char[map.width, map.height]; ; // denna array kommer innehålla alla chars från mapArray fast tvådimensionellt vilket gör det lättare att hålla reda på

                for (int y = 0; y < map.height; y++) // kollar kombinationer av x- och y-koordinater
                {
                    for (int x = 0; x < map.width; x++) // ^
                    {
                        // fullMapArray är tom och fylls på en koordinat i taget med nästa värde i mapArray
                        fullMapArray[x, y] = mapArray[(map.width * y) + x]; // eftersom att mapArray är endimensionell används [(map.width * y) + x] för att veta vilken del av arrayen som lagras
                    }
                }

                return fullMapArray; // den tvådimensionella arrayen returneras
            }
        }

        public static void ColorMap(char[,] tiles, int x, int y, string state)
        {// måste vara public static för att inte krascha

            if (state == "update") // update betyder att det är spelarens position som markeras för målning
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
            if (x == 0 && y == 0 && state == "init") // init betyder att det är när kartan ritas
            {

            }
            if (x != 0 || y != 0 && state == "init")
            {
                Console.Write(" ");
            }
        }
    }
}
