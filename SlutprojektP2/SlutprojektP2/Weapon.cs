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
        protected string type;

        public int Damage
        {
            get { return damage; }   // get method
            private set { damage = value; }   // set method
        }

        public string Type
        {
            get { return type; }   // get method
            set { type = value; }   // set method
        }

        public static void Bow(Character c) // skapar en båge
        {
            Weapon bow = new Weapon() { Damage = 3, type = "Bow"};

            c.CurrentWeapon = bow;
        }

        public static void Sword(Character c)
        {
            Weapon sword = new Weapon() { Damage = 5, type = "Sword" };

            c.CurrentWeapon = sword;
        }

        public static void Mace(Character c)
        {
            Weapon mace = new Weapon() { Damage = 7, type = "Mace" };

            c.CurrentWeapon = mace;
        }
    }
}
