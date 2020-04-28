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

        public Battle(Player player, Enemy enemy) // väldigt simpel metod, man klickar vidare och hoppas på att vinna :)
        {
            List<Character> characters = new List<Character>();
            characters.Add(player);
            characters.Add(enemy);
            // se där!        ^ lite polymorfism :)

            // detta är hårdkodat vilket gör att om jag implementerar en till fiendetyp i Games enemylista måste jag ändra här också (inte optimalt)
            if (enemy.Name == "Warrior")
            {
                Weapon.Mace(enemy); // warrior får en mace
            }
            else
            {
                Weapon.Bow(enemy); // archer får en bow
            }
            
            while (!isOver) // battle game loop
            {
                Console.Clear();

                foreach (Character c in characters) // polymorfismen är användbar här
                {
                    Console.WriteLine("{0}'s turn!", c.Name);
                    Console.WriteLine("\n{0}'s stats:\nHP: {1} \nDodge Chance: {2}%\nArmor: {3}", c.Name, c.HP, c.DodgeChance, c.Armor);
                    Console.WriteLine("\nWeapon stats: \nType: {0} \nDamage: {1}", c.CurrentWeapon.Type, c.CurrentWeapon.Damage);
                    Console.WriteLine();

                    if (c.Name == "Player")
                    {
                        characters[0].Attack(characters[1]); // player attackerar enemy
                    }
                    else
                    {
                        characters[1].Attack(characters[0]); // enemy attackerar player
                    }

                    Console.WriteLine("Press [ENTER] to continue");
                    Console.WriteLine("____________________________________________");
                    Console.ReadLine();
                }

                if (enemy.HP <= 0 || player.HP <= 0) // om någon är död
                {
                    isOver = true;

                    switch (player.HP)
                    {
                        case var expression when player.HP > 0:
                            Game.messages.Enqueue(string.Format("You beat the {0}!      ", enemy.Name)); // player vann
                            player.CurrentXp += enemy.MaxHP * 2;
                            switch (player.CurrentXp)
                            {
                                case var expression2 when player.CurrentXp >= player.XpToLevelUp: // playerns xp är nog för att
                                    player.LevelUp(player); // player levlar upp
                                    break;
                                case var expression2 when player.CurrentXp < player.XpToLevelUp: // playerns xp är inte nog för att levla upp
                                    break;
                            }
                            break;
                        case var expression when player.HP < 0:
                            Game.messages.Enqueue(string.Format("You were killed by the {0}       ", enemy.Name)); // player dog och battlet samt spelet är över
                            break;
                    }
                }
            }
        }
    }
}
