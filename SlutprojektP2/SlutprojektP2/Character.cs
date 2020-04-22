using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutprojektP2
{
    abstract class Character
    {
        // alla variabler är protected för att varje character ska ha dem
        protected string name;
        protected int maxHp;
        protected int hp;
        protected int xpToLevelUp;
        protected int currentXp;
        protected int armor;
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

        public int[] Pos  // property
        {
            get { return position; }   // get method
            set { position = value; }   // set method
        }

        public int XpToLevelUp  
        {
            get { return xpToLevelUp; }   
            set { xpToLevelUp = value; }  
        }

        public int CurrentXp 
        {
            get { return currentXp; }  
            set { currentXp = value; } 
        }

        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        public int Level
        {
            get {return level; }
            set {level = value; }
        }

        public int MaxHP
        {
            get { return maxHp; }
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
            get { return name; }  
            set { name = value; }  
        }

        // just nu använder bara player denna metod för att levla upp
        public void LevelUp(Character c) // skulle nog kunna göras virtual för att kunna ändras för olika sorters karaktärer
        {//                  ^ polymorfism :)
            c.Level++;
            c.currentXp = 0;
            c.XpToLevelUp = 10 + (5 * c.Level);
            c.MaxHP = 10 + (3 * c.Level);
            c.HP = c.MaxHP;
            Game.messages.Enqueue("Level Up!");
        }

        public void Attack(Character attacker, Character opponent) // simpel attackmetod
        {//                  ^   polymorfism :)   ^
            if (Game.gen.Next(100) > (opponent.dodgeChance))
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
