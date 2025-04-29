using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{   
    // Inherits from monster:
    public class Hound : Monster
    {
        // All monsters have unique values. Being minimum HP, maximum HP, minimum attack damage, and maximum attack damage. This allows for randomness:
        public Hound() : base("Hound", 50, 60, 4, 10)
        {
            GoesFirst = random.Next(4) == 1;
        }
        public override void Collect()
        {
            base.Collect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            this.PrintDelay("> The Hound has been slain.\nIts angry soul is absorbed by the brave explorer...\n", 1);
            Console.ForegroundColor = ConsoleColor.Gray;
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }
        public override string GetMonsterNoise()
        {
            return ("You hear the barking of a fierce Hound...\n");
        }
    }
}