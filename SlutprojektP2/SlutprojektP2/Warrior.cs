using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutprojektP2
{
    class Warrior : Enemy
    {
        public Warrior(Player player)
        {
            maxHp = 6 + (4 * player.Level);
            hp = maxHp;
            name = "Warrior";
            DodgeChance = 15;
            Armor = 3;
        }
    }
}
