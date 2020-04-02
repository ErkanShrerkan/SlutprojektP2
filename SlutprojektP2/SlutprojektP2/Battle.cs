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
            List<Character> characters = new List<Character>();
            characters.Add(player);
            characters.Add(enemy);

            if (enemy.Name == "Warrior")
            {
                Weapon.Mace(enemy); // warrior får en mace
            }
            else
            {
                Weapon.Bow(enemy); // archer får en bow
            }

            while (!isOver)
            {
                Console.Clear();
                //Console.WriteLine("welcome to battle yes, {0} and {1}", player.Name, enemy.Name);
                //Console.WriteLine("\n{0} hp: {1}\n{2} hp: {3}", player.Name, player.HP, enemy.Name, enemy.HP);

                foreach (Character c in characters)
                {
                    Console.WriteLine("{0}'s turn!", c.Name);
                    Console.WriteLine("\n{0}'s stats:\nHP: {1} \nDodge Chance: {2}%\nArmor: {3}", c.Name, c.HP, c.DodgeChance, c.Armor);
                    Console.WriteLine("\nWeapon stats: \nType: {0} \nDamage: {1}", c.CurrentWeapon.Type, c.CurrentWeapon.Damage);
                    Console.WriteLine("\nPress [ENTER] to continue");
                    Console.ReadLine();

                    if (c.Name == "Player")
                    {
                        characters[0].Attack(characters[0], characters[1]); // player attackerar enemy
                    }
                    else
                    {
                        characters[1].Attack(characters[1], characters[0]);
                    }
                }

                if (enemy.HP <= 0 || player.HP <= 0) // någon är död
                {
                    isOver = true;

                    switch (player.HP)
                    {
                        case var expression when player.HP > 0:
                            Game.messages.Enqueue(string.Format("You beat the {0}!", enemy.Name)); // player vann
                            player.xp += enemy.MaxHP;
                            player.HP = player.MaxHP;
                            break;
                        case var expression when player.HP < 0:
                            Game.messages.Enqueue(string.Format("You were killed by the {0}", enemy.Name)); // player dog
                            break;
                    }
                }
            }
        }
    }
}
