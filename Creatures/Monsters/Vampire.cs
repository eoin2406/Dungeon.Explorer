using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    // Inherits from monster:
    public class Vampire : Monster
    {
        // All monsters have unique values. Being minimum HP, maximum HP, minimum attack damage, and maximum attack damage. This allows for randomness:
        public Vampire() : base("Vampire", 50, 70, 6, 14)
        {
            GoesFirst = random.Next(3) == 1;
        }
        public override void Collect()
        {
            base.Collect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            this.PrintDelay("> The Vampire falls down.\nIts rotten soul is absorbed by the brave explorer...\n", 1);
            Console.ForegroundColor = ConsoleColor.White;
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }
        public override string GetMonsterNoise()
        {
            return ("You hear a Vampire laughing...\n");
        }
    }
}