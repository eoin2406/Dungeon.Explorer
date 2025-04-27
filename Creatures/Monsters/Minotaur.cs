using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    // This is the boss monster. It cannot spawn in rooms, and can only be battled in the boss room. Its minimum and maximum health are the same every game:
    public class Minotaur : Monster
    {
        public Minotaur() : base("Minotaur", 300, 300, 35, 50)
        {
            GoesFirst = true;
        }
        public override void Collect()
        {
            base.Collect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            this.PrintDelay("The Minotaur has fallen. Its cursed soul absorbed by the brave explorer...\n", 1);
            Console.ForegroundColor = ConsoleColor.White;
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }
        public override string GetMonsterNoise()
        {
            return ("The Minotaur roars at you, fueled by anger and hatred...\n");
        }
    }
}