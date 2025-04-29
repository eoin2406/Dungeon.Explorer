using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{  
    // Inherits from monster:
    public class Goblin : Monster
    {
        // All monsters have unique values. Being minimum HP, maximum HP, minimum attack damage, and maximum attack damage. This allows for randomness:
        public Goblin() : base("Goblin", 40, 50, 3, 7)
        {
            GoesFirst = random.Next(4) == 1;
        }
        public override void Collect()
        {
            base.Collect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            this.PrintDelay("> The Goblin has been defeated.\n Its frail soul is absorbed by the brave explorer...\n", 1);
            Console.ForegroundColor = ConsoleColor.Gray;
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }
        public override string GetMonsterNoise()
        {
            return ("A puny Goblin screeches at you...\n");
        }
    }
}