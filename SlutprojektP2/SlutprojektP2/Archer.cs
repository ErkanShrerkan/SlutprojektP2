using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutprojektP2
{
    class Archer : Enemy
    {
        public Archer(Player player)
        {
            maxHp = 5 + (2 * player.Level);
            hp = maxHp;
            name = "Archer";
            DodgeChance = 30;
            Armor = 1;
        }
    }
}
