using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutprojektP2
{
    class Battle
    {
        bool isOver = false;

        public Battle(Player player, Enemy enemy)
        {
            while (!isOver)
            {
                Console.Clear();
                Console.WriteLine("welcome to battle yes, {0} and {1}", player.Name, enemy.Name);
                Console.ReadLine();
            }
        }
    }
}
