using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutprojektP2
{
    class Weapon : Item
    {
        protected int damage;

        public int Damage
        {
            get { return damage; }   // get method
            set { damage = value; }   // set method
        }
    }
}
