using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutprojektP2
{
    abstract class Character
    {
        protected bool collision;
        protected int layer;
        protected string sprite;
        protected string name;
        protected int hp;
        protected int armor;
        protected int damage;
        protected int dodgeChance;
        protected Weapon currentWeapon;
        protected bool up;
        protected bool down;
        protected bool left;
        protected bool right;
        protected bool[] directions;
        protected char[] collidableTiles;
        protected int[] position = new int[2];

        public int[] Pos  // property
        {
            get { return position; }   // get method
            set { position = value; }   // set method
        }

        protected void Attack(Character opponent)
        {
            if (Game.gen.Next(101) >= (opponent.dodgeChance))
            {
                //hit
                if ((currentWeapon.Damage - opponent.armor / 2) <= 0)
                {
                    opponent.hp -= 1;
                }
                else
                {
                    opponent.hp -= (currentWeapon.Damage - opponent.armor);
                }
            }
            else
            {
                // miss
            }
        }

        public string Name
        {
            get { return name; }   // get method
            set { name = value; }   // set method
        }
    }
}
