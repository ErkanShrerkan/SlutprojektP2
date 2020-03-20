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
                Console.WriteLine("\n{0} hp: {1}\n{2} hp: {3}", player.Name, player.HP, enemy.Name, enemy.HP);
                Console.ReadLine();
                enemy.HP -= 3;

                if (enemy.HP <= 0 || player.HP <= 0)
                {
                    isOver = true;

                    switch (player.HP)
                    {
                        case var expression when player.HP > 0:
                            Game.messages.Enqueue(string.Format("You beat the {0}!", enemy.Name));
                            break;
                        case var expression when player.HP < 0:
                            Game.messages.Enqueue(string.Format("You were killed by the {0}", enemy.Name));
                            break;
                    }
                }
            }
        }
    }
}
