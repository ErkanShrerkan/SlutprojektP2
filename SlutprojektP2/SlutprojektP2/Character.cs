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
        protected int maxHp;
        protected int hp;
        protected int xpToLevelUp;
        protected int currentXp;
        protected int armor;
        protected int damage;
        protected int dodgeChance;
        protected Weapon currentWeapon;
        protected int level;
        protected bool up;
        protected bool down;
        protected bool left;
        protected bool right;
        protected bool[] directions;
        protected char[] collidableTiles;
        protected int[] position = new int[2];
        protected int[] lastPosition = new int[2];
        protected int[] currentPosition = new int[2];

        public int[] Pos  // property
        {
            get { return position; }   // get method
            set { position = value; }   // set method
        }

        public int[] LastPos  // property
        {
            get { return lastPosition; }   // get method
            set { lastPosition = value; }   // set method
        }

        public int[] CurrentPos  // property
        {
            get { return currentPosition; }   // get method
            set { currentPosition = value; }   // set method
        }

        public int XpToLevelUp  // property
        {
            get { return xpToLevelUp; }   // get method
            set { xpToLevelUp = value; }   // set method
        }

        public int CurrentXp  // property
        {
            get { return currentXp; }   // get method
            set { currentXp = value; }   // set method
        }

        public int HP  // property
        {
            get { return hp; }   // get method
            set { hp = value; }   // set method
        }

        public int Level
        {
            get {return level; }
            set {level = value; }
        }

        public int MaxHP  // property
        {
            get { return maxHp; } // get method
            set { maxHp = value; }
        }

        public Weapon CurrentWeapon
        {
            get { return currentWeapon; }
            set { currentWeapon = value; }
        }

        public int DodgeChance
        {
            get { return dodgeChance; }
            set { dodgeChance = value; }
        }

        public int Armor
        {
            get { return armor; }
            set { armor = value; }
        }

        public string Name
        {
            get { return name; }   // get method
            set { name = value; }   // set method
        }

        // just nu använder bara player denna metod för att levla upp
        public void LevelUp(Character c) // skulle kunna göras virtual för att kunna ändras för olika sorters karaktärer
        {
            c.Level++;
            c.currentXp = 0;
            c.XpToLevelUp = 10 + (5 * c.Level);
            c.MaxHP = 10 + (3 * c.Level);
            c.HP = c.MaxHP;
            Game.messages.Enqueue("Level Up!");
        }

        public void Attack(Character attacker, Character opponent)
        {
            if (Game.gen.Next(101) >= (opponent.dodgeChance))
            {
                //hit
                if ((currentWeapon.Damage - opponent.armor / 2) <= 0)
                {
                    opponent.hp -= 1;
                    Console.WriteLine("{0} was hit for 1 damage!\n", opponent.Name);
                }
                else
                {
                    opponent.hp -= (currentWeapon.Damage - opponent.armor / 2);
                    Console.WriteLine("{0} was hit for {1} damage!\n", opponent.Name, (currentWeapon.Damage - opponent.armor));
                }
            }
            else
            {
                // miss
                Console.WriteLine("{0} missed!\n", attacker.Name);
            }
        }
    }
}
